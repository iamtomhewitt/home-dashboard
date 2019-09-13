SCHEDULER.every "1m", :first_in => 0 do |job|

    items = []
    items.push ({
        name: "Item Name",
        removeUrl: "<a href='http://www.google.com' target='_blank'>Item URL</a>"
    });

    send_event('todo-list', items: items)
end