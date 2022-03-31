//Tooltip trigger
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="modal"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
});
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
});

//Pass asp-route-id to remove modal
if (document.getElementById('removeModal') !== null) {
    var removeModal = document.getElementById('removeModal');
    const url = document.getElementById("recordDelForm").action;

    removeModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        var recordId = button.getAttribute('data-bs-whatever');
        document.getElementById("recordDelForm").action = url + "/" + recordId;
    });
}

//Pass asp-route-id to send email modal
if (document.getElementById('sendEmailModal') !== null) {
    var sendEmailModal = document.getElementById('sendEmailModal');
    const url = document.getElementById("recordSendForm").action;

    sendEmailModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        var recordId = button.getAttribute('data-bs-whatever');
        var emailRec = button.getAttribute('data-bs-whatever2');
        document.getElementById("recordSendForm").action = url + "/" + recordId;
        document.getElementById("emailReceiver").value = emailRec;
    });
}
