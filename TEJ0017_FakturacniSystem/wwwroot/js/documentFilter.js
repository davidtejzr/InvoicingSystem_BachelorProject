const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);

if (urlParams.has('filterRadioPaid')) {
    if (urlParams.get('filterRadioPaid') === 'all')
        document.getElementById('checkBoxAll').checked = true;
    else if (urlParams.get('filterRadioPaid') === 'paid')
        document.getElementById('checkBoxPaid').checked = true;
    else if (urlParams.get('filterRadioPaid') === 'unpaid')
        document.getElementById('checkBoxNotPaid').checked = true;
}

if (urlParams.has('filterMinDatetime')) {
    document.getElementById('filterMinDatetime').value = urlParams.get('filterMinDatetime');
}

if (urlParams.has('filterMaxDatetime')) {
    document.getElementById('filterMaxDatetime').value = urlParams.get('filterMaxDatetime');
}

if (urlParams.has('filterCustomerSelect')) {
    document.getElementById('filterCustomerSelect').value = urlParams.get('filterCustomerSelect');
}