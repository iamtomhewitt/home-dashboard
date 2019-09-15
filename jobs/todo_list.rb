require 'json'
require 'net/http'
require 'httparty'

private_code    = 'BUmbxItLy0a8oq8Gtz1_xQhaHmJ5QbCUKnqfRKGMs85A'
public_code     = '5d7bc377d1041303ec9461d6'

get_url         = 'http://dreamlo.com/lb/'+public_code+'/json'
add_url         = 'http://dreamlo.com/lb/'+private_code+'/add/'
delete_url      = 'http://dreamlo.com/lb/'+private_code+'/delete/'

SCHEDULER.every "15s", :first_in => 0 do |job |
    items = []

    uri = URI.parse(get_url)
    response = Net::HTTP.get_response(uri)
    data = JSON.parse(response.body)

    if !data['dreamlo']['leaderboard'].nil?
        entries = data['dreamlo']['leaderboard']['entry']

        if entries.kind_of?(Array)
            entries.each do |child|
                item_name = child['name']

                # a very long stir <- the longest a string can be before it overflows
                # todo replace the last the characters with ... if over a certain length

                # TODO make it not open the link in a new tab
                delete_command = """
                    <a onclick='submit("""+item_name+""")'>Delete</a>
                    <script type='text/javascript'>
                        function submit(name) {
                            alert('Hello there!');
                            var xmlHttp = new XMLHttpRequest();
                            xmlHttp.onreadystatechange = function() { 
                                if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
                                    callback(xmlHttp.responseText);
                            }
                            xmlHttp.open('GET', "+delete_url+"name, true);
                            xmlHttp.send(null);
                        }
                    </script>
                """

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