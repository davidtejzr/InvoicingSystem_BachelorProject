// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const apiTalksKey = "niBKjxQ9Kz4UFtvodljP08vw44xYMHchH7Ipqf74";

function showModal() {
    $('#myModal').modal('show');
}

//tooltips
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

//delaying input typing
function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

function getDataFromAresByIco() {
    const ico = document.getElementById("SubjectIco").value;
    document.getElementById("msgErrDiv").style = "display: none !important;"
    document.getElementById("msgInfoDiv").style = "display: none !important;"

    if (ico.length == 8) {
        $.ajax({
            type: "GET",
            url: "/AddressBook/AresDataIco",
            dataType: "json",
            data: {
                ico: ico
            }

        }).done(function (data) {
            console.log(data);
            if (data["ErrorMsg"]) {
                document.getElementById("msgErrDiv").style = "display: relative;"
                document.getElementById("msgErrText").innerHTML = data["ErrorMsg"];
            }
            else {
                document.getElementById("SubjectSearchInput").value = data["SubjectName"]
                document.getElementById("SubjectDic").value = "CZ" + ico;
                document.getElementById("SubjectStreet").value = data["SubjectStreet"]
                document.getElementById("SubjectHouseNumber").value = data["SubjectHouseNumber"]
                document.getElementById("SubjectCity").value = data["SubjectCity"]
                document.getElementById("SubjectZip").value = data["SubjectZip"]

                document.getElementById("msgInfoDiv").style = "display: relative;"
                document.getElementById("msgInfoText").innerHTML = data["InfoMsg"];
            }
        });
    }
    else {
        document.getElementById("msgErrDiv").style = "display: relative;";
        document.getElementById("msgErrText").innerHTML = "Chybný formát IČO";
    }  
}

$('#SubjectSearchInput').keyup(delay(function (e) {
    const searchString = document.getElementById("SubjectSearchInput").value

    if (searchString.length > 3) {
        $.ajax({
            type: "GET",
            url: "/AddressBook/AresDataName",
            dataType: "json",
            data: {
                name: searchString
            }

        }).done(function (data) {
            console.log(data);

            document.getElementById("SubjectSearchResult").innerHTML = "";
            Object.keys(data).forEach(function (key) {
                let item = document.createElement("a");
                item.className = "list-group-item list-group-item-action";
                item.innerHTML = key;
                item.value = data[key];
                item.onclick = function () {
                    document.getElementById("SubjectSearchResult").innerHTML = "";
                    document.getElementById("SubjectSearchResult").value = item.innerHTML;
                    document.getElementById("SubjectIco").value = item.value
                    getDataFromAresByIco()
                };
                document.getElementById("SubjectSearchResult").appendChild(item);
            });

        });
    }
}, 500));

