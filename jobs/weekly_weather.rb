require "net/http"
require "json"

# If these are left blank, the script will try to locate you using your IP address
latitude = "53.848246"                   # Required
longitude = "-1.631747"                  # Required
location = "Horsforth,<br>Leeds"   # Change me

units = "uk"
symbol = "C"
key = "23e9dad33a0bffa114dd52285c2807ad"                        # Required

def ftoc (f)
    c = (f-32)*5/9
end
  
SCHEDULER.every "15m", :first_in => 0 do |job|

    uri = URI("https://api.darksky.net/forecast/#{key}/#{latitude},#{longitude}?units=#{units}")
    req = Net::HTTP::Get.new(uri.path)

    # Make request
    res = Net::HTTP.start(
            uri.host, uri.port, 
            :use_ssl => uri.scheme == 'https', 
            :verify_mode => OpenSSL::SSL::VERIFY_NONE) do |https|
      https.request(req)
    end

    response = JSON.parse res.body
    
    currentResults = response["currently"]
    dailyResults  = response["daily"]["data"]
        
    forecasts = []
    
    #Today
    today = {}
    
    if currentResults
        
        currentTemp = symbol == "C" ? "#{ftoc(currentResults["temperature"]).round}°#{symbol}" : "#{currentResults["temperature"].round}°#{symbol}"
        currentlyIcon = currentResults["icon"]
        currentHigh = ftoc(dailyResults[0]["temperatureMax"]).round
        currentLow = ftoc(dailyResults[0]["temperatureMin"]).round
        currentSummary = response["hourly"]["summary"]
        todaysSummary = "High of #{currentHigh} with a low of #{currentLow}. #{currentSummary}"
            
        # Create object for this current day
        today = {
                temp: currentTemp,
                summary:  todaysSummary,
                code: currentlyIcon,
                element: 'currentWeatherIcon',
                location: location
            }
    end
    
    #Future Days

    if dailyResults
        # Create weather object for the next 5 days
        for day in (1..5)         
            day = dailyResults[day]
        
            # Format date as a qualified day i.e. Monday
            time = Time.at(day["time"]).strftime("%A")
            summary = day["summary"]
            
            # Should it be displayed in Celsius? If not, display in Fahrenheit
            if(symbol == "C")
                min = ftoc(day["temperatureMin"])
                max = ftoc(day["temperatureMax"])
            else
                min = day["temperatureMin"]
                max = day["temperatureMax"]
            end
                
            # Create object for the day to send back to the widget
            this_day = {
                high: max.round,
                low:  min.round,
                date: time,
                code: day["icon"],
                text: day["text"], 
                
                element: 'weather-icon'
            }
            forecasts.push(this_day)
        end

        send_event "weeklyweather", { forecasts: forecasts, today: today }
    end
end