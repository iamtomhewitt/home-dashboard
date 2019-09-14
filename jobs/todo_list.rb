require 'json'
require 'net/http'
require 'httparty'

private_code    = 'BUmbxItLy0a8oq8Gtz1_xQhaHmJ5QbCUKnqfRKGMs85A'
public_code     = '5d7bc377d1041303ec9461d6'

get_url         = 'http://dreamlo.com/lb/'+public_code+'/json'
add_url         = 'http://dreamlo.com/lb/'+private_code+'/add/'
delete_url      = 'http://dreamlo.com/lb/'+private_code+'/delete/'

SCHEDULER.every "1m", :first_in => 0 do |job |
    items = []

    uri = URI.parse(get_url)
    response = Net::HTTP.get_response(uri)
    data = JSON.parse(response.body)

    if !data['dreamlo']['leaderboard'].nil?
        entries = data['dreamlo']['leaderboard']['entry']

        if entries.kind_of?(Array)
            entries.each do |child|
                item_name = child['name']
                items.push({
                    name: item_name,
                    deleteUrl: "<a href='"+delete_url+item_name+"' target='_blank'>Delete</a>"
                });
            end
        else
            item_name = entries['name']
            items.push({
                name: item_name,
                deleteUrl: "<a href='"+delete_url+item_name+"' target='_blank'>Delete</a>"
            });
        end
    end

    send_event('todo-list', items: items)
end