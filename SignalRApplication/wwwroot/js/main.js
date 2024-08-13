(function ($) {

    'use strict';

    let connection = new signalR
        .HubConnectionBuilder()
        .withUrl('/signalRServer')
        .build();

    connection.start();
    connection.on('refreshTypes', function () {
        loadTypes();
    });

    async function loadTypes() {

        const response = await fetch('http://localhost:5163/api/products')
        const data = await response.json();

        const template = data.map(item => {





        });

        console.log(template);
    }

    loadTypes();
})(jQuery);           