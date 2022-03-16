// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const apiTalksKey = "niBKjxQ9Kz4UFtvodljP08vw44xYMHchH7Ipqf74";

function showModal() {
    $('#myModal').modal('show');
}

function getStatesFromApi() {
    const stateSelect = document.getElementById("stateSelector");
    console.log("ahoj");

    $.ajax({
        type: "GET",
        url: "https://api.apitalks.store/czso.cz/stat",
        headers: { "x-api-key": apiTalksKey }
    }).done(function (data) {
        console.log(data);
        for (let i = 0; i < data['results'].length; i++) {

        }
    });
}