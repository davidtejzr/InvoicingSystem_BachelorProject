//Tooltip trigger
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="modal"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
});

//Pass asp-route-id to modal
var removeModal = document.getElementById('removeModal');
const url = document.getElementById("recordDelForm").action;
removeModal.addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var recordId = button.getAttribute('data-bs-whatever');
    document.getElementById("recordDelForm").action = url + "/" + recordId;
    });