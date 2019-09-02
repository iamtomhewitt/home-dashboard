require 'icalendar'

ical_url_tom = 'https://calendar.google.com/calendar/ical/tommhewitt%40gmail.com/private-0d6bbecbad07707c3e0f4f2a4bbdd5df/basic.ics'
ical_url_lauren = 'https://calendar.google.com/calendar/ical/laurencolesyt%40gmail.com/private-b0686a8fb9597e6625bdd938e20f3653/basic.ics'

uri_tom = URI ical_url_tom
uri_lauren = URI ical_url_lauren

def get_calendar_events(widget_name, url)
  # puts url
  result = Net::HTTP.get url
  calendars = Icalendar::Calendar.parse(result)
  calendar = calendars.first

  events = calendar.events.map do |event|
    {
      start: event.dtstart,
      end: event.dtend,
      summary: event.summary[0..21]
    }
  end.select { |event| event[:start] > DateTime.now }

  events = events.sort { |a, b| a[:start] <=> b[:start] }
  events = events[0..5]
  # puts events

  send_event(widget_name, { events: events })
end

SCHEDULER.every '5m', :first_in => 4 do |job|
  get_calendar_events('google_calendar_tom', uri_tom);
  get_calendar_events('google_calendar_lauren', uri_lauren)
end