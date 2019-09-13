require 'json'
require 'net/http'
require 'httparty'

private_code = 'BUmbxItLy0a8oq8Gtz1_xQhaHmJ5QbCUKnqfRKGMs85A'
public_code = '5d7bc377d1041303ec9461d6'

add_url = 'http://dreamlo.com/lb/BUmbxItLy0a8oq8Gtz1_xQhaHmJ5QbCUKnqfRKGMs85A/add/'
get_url = 'http://dreamlo.com/lb/5d7bc377d1041303ec9461d6/json'
delete_url = 'http://dreamlo.com/lb/BUmbxItLy0a8oq8Gtz1_xQhaHmJ5QbCUKnqfRKGMs85A/delete/'

def fetch(uri)
    response = HTTParty.get(uri)
    data = response['dreamlo']['leaderboard']['entry']
end

SCHEDULER.every "1m", :first_in => 0 do |job |
    items = []

    data = fetch(get_url)
    data.each do |item|
        items.push({
        name: item['name'],
        deleteUrl: "<a href='"+delete_url+item['name']+"' target='_blank'>Delete</a>"
    });
    end

    send_event('todo-list', items: items)
end