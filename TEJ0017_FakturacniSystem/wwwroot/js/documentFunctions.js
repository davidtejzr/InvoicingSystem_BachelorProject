let rowIndexCounter = 0;
let rowIndexes = [];
let sum = 0;
let sumWoDph = 0;
let isWithVat = 0;
let totalString = "Celková cena: ";

function renderVatSummary(vatsDict) {
    const vatSummaryContainer = document.getElementById("vatTable");
    vatSummaryContainer.innerHTML = "";

    const table = document.createElement("table");
    table.className = "table table-striped";
    table.style.minWidth = "300px";

    const tableTbody = document.createElement("tbody");

    sum = 0;
    for (const key in vatsDict) {
        const vatKey = document.createElement("th");
        if (key == "total") {
            vatKey.innerHTML = "Celkem bez DPH";
        }
        else {
            vatKey.innerHTML = "DPH " + key + "%";
        }
        const vatValue = document.createElement("td");
        vatValue.innerHTML = vatsDict[key] + ",- Kč";

        const oneRow = document.createElement("tr");
        oneRow.appendChild(vatKey);
        oneRow.appendChild(vatValue);

        tableTbody.appendChild(oneRow);

        sum += vatsDict[key];
    }

    table.appendChild(tableTbody);
    vatSummaryContainer.appendChild(table);
}

function setVatDocument(isVatDocument) {
    if (isVatDocument === "True") {
        isWithVat = 1;
    }
    else {
        isWithVat = 0;
    }
}

function calcTotalAmount() {
    sumWoDph = 0;
    sum = 0;
    var vatsDict = {};
    for (var item in rowIndexes) {
        var rowItem = document.getElementById(rowIndexes[item]);
        if (isWithVat === 1) {
            //vat summary
            let currentKey = rowItem.childNodes[4].childNodes[0].value;
            let computedValue = parseFloat(vatsDict[currentKey]);
            if (vatsDict[currentKey] === undefined) {
                computedValue = Math.round(parseFloat(rowItem.childNodes[1].childNodes[0].value) * parseFloat(rowItem.childNodes[2].childNodes[0].value) * (parseFloat(rowItem.childNodes[4].childNodes[0].value) / 100) * 100) / 100;
                vatsDict[currentKey] = computedValue;
            }
            else {
                computedValue += Math.round(parseFloat(rowItem.childNodes[1].childNodes[0].value) * parseFloat(rowItem.childNodes[2].childNodes[0].value) * (parseFloat(rowItem.childNodes[4].childNodes[0].value) / 100) * 100) / 100;
                vatsDict[currentKey] = computedValue;
            }

            const amount = rowItem.childNodes[5].childNodes[0].value;
            sumWoDph += parseFloat(amount);
        }
        else {
            const amount = rowItem.childNodes[4].childNodes[0].value;
            sumWoDph += parseFloat(amount);
            sum += parseFloat(amount);
        }
    }

    if (isWithVat === 1) {
        vatsDict["total"] = sumWoDph;
        renderVatSummary(vatsDict);
        totalString = "Celková cena s DPH: "
    }

    CalcDiscount();
}

function CalcDiscount() {
    const discountInputAmount = document.getElementById("discountInputAmount").value;
    const discountAmount = document.getElementById("discountAmount");

    if (parseFloat(discountInputAmount) > 0) {
        //round to 2 decimal
        const computedDiscount = Math.round(-(sum * (discountInputAmount / 100)) * 100)/100;
        discountAmount.innerHTML = parseFloat(computedDiscount) + ",- Kč";
        const sumWithDiscount = Math.round((sum + computedDiscount) * 100) / 100;
        document.getElementById("totalAmount").innerHTML = totalString + sumWithDiscount + ",- Kč";
    }
    else {
        discountAmount.innerHTML = "0.0,- Kč";
        const sumRound = Math.round(sum * 100) / 100;
        document.getElementById("totalAmount").innerHTML = totalString + sumRound + ",- Kč";
    }
}

function recalculateAmount(itemId) {
    var rowItem = document.getElementById(itemId);
    const price = rowItem.childNodes[1].childNodes[0].value;
    const amount = rowItem.childNodes[2].childNodes[0].value;
    if (isWithVat === 1) {
        const vat = rowItem.childNodes[4].childNodes[0].value;
        rowItem.childNodes[5].childNodes[0].value = Math.round((price * amount) * 100) / 100;
    }
    else {
        rowItem.childNodes[4].childNodes[0].value = Math.round((price * amount) * 100) / 100;
    }
    calcTotalAmount();
}

function recalculateAll() {
    for (var item in rowIndexes) {
        recalculateAmount(rowIndexes[item]);
    }
}

function DocumentAddItem(defaultMJ, defaultVat) {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
    rowIndexes.push("item" + rowIndexCounter);
    const itemId = "item" + rowIndexCounter++;

    //items
    const removeButton = document.createElement("button");
    const removeIcon = document.createElement("i");
    removeIcon.className = "bi bi-trash2";
    removeButton.appendChild(removeIcon);
    removeButton.className = "btn btn-outline-danger";
    removeButton.type = "button";
    removeButton.id = itemId;
    removeButton.onclick = function () {
        RemoveItemId(removeButton.id);
    };

    const ItemNameInput = document.createElement("input");
    ItemNameInput.id = itemId + "_name";
    ItemNameInput.name = "ItemName";
    ItemNameInput.className = "form-control";
    ItemNameInput.required = true;
    ItemNameInput.autocomplete = "off";
    ItemNameInput.onkeyup = function () {
        FillFromItemsList(itemId);
    };

    const ItemSearchResult = document.createElement("div");
    ItemSearchResult.id = itemId + "_itemSearchResult";
    ItemSearchResult.className = "list-group"

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    //ItemPriceInput.min = 0;
    ItemPriceInput.step = "0.01";
    ItemPriceInput.value = "0.00"
    ItemPriceInput.onchange = function () {
        recalculateAmount(itemId);
    }

    const ItemAmountInput = document.createElement("input");
    ItemAmountInput.id = itemId + "_amount";
    ItemAmountInput.name = "ItemAmount";
    ItemAmountInput.className = "form-control";
    ItemAmountInput.type = "number";
    ItemAmountInput.value = 1;
    ItemAmountInput.min = 1;
    ItemAmountInput.onchange = function () {
        recalculateAmount(itemId);
    }

    const ItemUnitInput = document.createElement("input");
    ItemUnitInput.id = itemId + "_unit";
    ItemUnitInput.name = "ItemUnit";
    ItemUnitInput.value = defaultMJ;
    ItemUnitInput.className = "form-control";

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowIndexCounter;
    ItemTotalAmount.className = "form-control";
    ItemTotalAmount.disabled = true;
    ItemTotalAmount.value = "0.00";

    //cols
    const removeButtonCol = document.createElement("div");
    removeButtonCol.className = "col-md-1";
    removeButtonCol.appendChild(removeButton);

    const ItemNameInputCol = document.createElement("div");
    ItemNameInputCol.className = "col-md-4";
    ItemNameInputCol.appendChild(ItemNameInput);
    ItemNameInputCol.appendChild(ItemSearchResult);

    const ItemPriceInputCol = document.createElement("div");
    ItemPriceInputCol.className = "col-md-2";
    ItemPriceInputCol.appendChild(ItemPriceInput);

    const ItemAmountInputCol = document.createElement("div");
    ItemAmountInputCol.className = "col";
    ItemAmountInputCol.appendChild(ItemAmountInput);

    const ItemUnitInputCol = document.createElement("div");
    ItemUnitInputCol.className = "col";
    ItemUnitInputCol.appendChild(ItemUnitInput);

    const ItemTotalAmountCol = document.createElement("div");
    ItemTotalAmountCol.className = "col";
    ItemTotalAmountCol.appendChild(ItemTotalAmount);


    //row
    const ItemRow = document.createElement("div");
    ItemRow.className = "row";
    ItemRow.style = "padding-top: 5px;"

    ItemRow.appendChild(ItemNameInputCol);
    ItemRow.appendChild(ItemPriceInputCol);
    ItemRow.appendChild(ItemAmountInputCol);
    ItemRow.appendChild(ItemUnitInputCol);

    //tax rates
    if (isWithVat === 1) {
        const ItemVat = document.createElement("input");
        ItemVat.id = itemId + "_vat";
        ItemVat.name = "ItemVat";
        ItemVat.value = defaultVat;
        ItemVat.min = 0;
        ItemVat.max = 100;
        ItemVat.type = "number";
        ItemVat.className = "form-control";
        ItemVat.onchange = function () {
            recalculateAmount(itemId);
        }

        const ItemVatCol = document.createElement("div");
        ItemVatCol.className = "col";
        ItemVatCol.appendChild(ItemVat);

        ItemRow.appendChild(ItemVatCol);
    }

    ItemRow.appendChild(ItemTotalAmountCol);
    ItemRow.appendChild(removeButtonCol);
    ItemRow.id = itemId;

    //parent
    const parent = document.getElementById("DocumentItemsBox");
    parent.appendChild(ItemRow);
}


function DocumentAddItemWithValues(name, price, amount, unit, vat) {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
    rowIndexes.push("item" + rowIndexCounter);
    const itemId = "item" + rowIndexCounter++;

    //items
    const removeButton = document.createElement("button");
    const removeIcon = document.createElement("i");
    removeIcon.className = "bi bi-trash2";
    removeButton.appendChild(removeIcon);
    removeButton.className = "btn btn-outline-danger";
    removeButton.type = "button";
    removeButton.id = itemId;
    removeButton.onclick = function () {
        RemoveItemId(removeButton.id);
    };


    const ItemNameInput = document.createElement("input");
    ItemNameInput.id = itemId + "_name";
    ItemNameInput.name = "ItemName";
    ItemNameInput.className = "form-control";
    ItemNameInput.required = true;
    ItemNameInput.value = name;
    ItemNameInput.onkeyup = function () {
        FillFromItemsList(itemId);
    };

    const ItemSearchResult = document.createElement("div");
    ItemSearchResult.id = itemId + "_itemSearchResult";
    ItemSearchResult.className = "list-group"

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    //ItemPriceInput.min = 0;
    ItemPriceInput.step = "0.01";
    ItemPriceInput.value = price;
    ItemPriceInput.onchange = function () {
        recalculateAmount(itemId);
    }

    const ItemAmountInput = document.createElement("input");
    ItemAmountInput.id = itemId + "_amount";
    ItemAmountInput.name = "ItemAmount";
    ItemAmountInput.className = "form-control";
    ItemAmountInput.type = "number";
    ItemAmountInput.value = amount;
    ItemAmountInput.min = 1;
    ItemAmountInput.onchange = function () {
        recalculateAmount(itemId);
    }

    const ItemUnitInput = document.createElement("input");
    ItemUnitInput.id = itemId + "_unit";
    ItemUnitInput.name = "ItemUnit";
    ItemUnitInput.className = "form-control";
    ItemUnitInput.value = unit;

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowIndexCounter;
    ItemTotalAmount.className = "form-control";
    ItemTotalAmount.disabled = true;
    ItemTotalAmount.value = "0.00";

    //cols
    const removeButtonCol = document.createElement("div");
    removeButtonCol.className = "col-md-1";
    removeButtonCol.appendChild(removeButton);

    const ItemNameInputCol = document.createElement("div");
    ItemNameInputCol.className = "col-md-4";
    ItemNameInputCol.appendChild(ItemNameInput);
    ItemNameInputCol.appendChild(ItemSearchResult);

    const ItemPriceInputCol = document.createElement("div");
    ItemPriceInputCol.className = "col-md-2";
    ItemPriceInputCol.appendChild(ItemPriceInput);

    const ItemAmountInputCol = document.createElement("div");
    ItemAmountInputCol.className = "col";
    ItemAmountInputCol.appendChild(ItemAmountInput);

    const ItemUnitInputCol = document.createElement("div");
    ItemUnitInputCol.className = "col";
    ItemUnitInputCol.appendChild(ItemUnitInput);

    const ItemTotalAmountCol = document.createElement("div");
    ItemTotalAmountCol.className = "col";
    ItemTotalAmountCol.appendChild(ItemTotalAmount);


    //row
    const ItemRow = document.createElement("div");
    ItemRow.className = "row";
    ItemRow.style = "padding-top: 5px;"

    ItemRow.appendChild(ItemNameInputCol);
    ItemRow.appendChild(ItemPriceInputCol);
    ItemRow.appendChild(ItemAmountInputCol);
    ItemRow.appendChild(ItemUnitInputCol);

    //tax rates
    if (isWithVat === 1) {
        const ItemVat = document.createElement("input");
        ItemVat.id = itemId + "_vat";
        ItemVat.name = "ItemVat";
        ItemVat.value = vat;
        ItemVat.min = 0;
        ItemVat.max = 100;
        ItemVat.type = "number";
        ItemVat.className = "form-control";
        ItemVat.onchange = function () {
            recalculateAmount(itemId);
        }

        const ItemVatCol = document.createElement("div");
        ItemVatCol.className = "col";
        ItemVatCol.appendChild(ItemVat);

        ItemRow.appendChild(ItemVatCol);
    }


    ItemRow.appendChild(ItemTotalAmountCol);
    ItemRow.appendChild(removeButtonCol);
    ItemRow.id = itemId;

    //parent
    const parent = document.getElementById("DocumentItemsBox");
    parent.appendChild(ItemRow);
}


function RemoveItemId(itemId) {
    if (rowIndexes.length > 1) {
        var rowItem = document.getElementById(itemId);
        rowItem.remove();
        for (var item in rowIndexes) {
            if (rowIndexes[item] === itemId)
                rowIndexes.splice(item, 1);
        }
        recalculateAll();
    }
    else {
        document.getElementById("DocumentWarningMessageDiv").style = "display: relative";
        document.getElementById("DocumentWarningMessage").innerHTML = "Doklad musí mít alespoň jednu položku!";
    }
}

function CustomerSelector() {
    var customerName = document.getElementById("customerSelector").value;

    $.ajax({
        type: "GET",
        url: "/BasicInvoices/CustomerData",
        dataType: "json",
        data: {
            customerName: customerName
        }

    }).done(function (data) {
        document.getElementById("labelCustomerAddressFirstRow").innerHTML = data["customerStreet"] + "&nbsp;" + data["customerHouseNumber"];
        document.getElementById("labelCustomerAddressSecondRow").innerHTML = data["customerZip"] + "&nbsp;&nbsp;" + data["customerCity"];
        document.getElementById("labelCustomerIco").innerHTML = "IČO: " + data["customerIco"];

        document.getElementById("CustomSubName").value = customerName;
        document.getElementById("CustomStreet").value = data["customerStreet"];
        document.getElementById("CustomHouseNumber").value = data["customerHouseNumber"];
        document.getElementById("CustomZip").value = data["customerZip"];
        document.getElementById("CustomCity").value = data["customerCity"];
        document.getElementById("CustomIco").value = data["customerIco"];

        if (data["customerDic"] !== null) {
            document.getElementById("labelCustomerDic").innerHTML = "DIČ: " + data["customerDic"];
            document.getElementById("CustomDic").value = data["customerDic"];
        }
        else
            document.getElementById("labelCustomerDic").innerHTML = "";
    });
}


function PaymentMethodSelector() {
    var paymentMethodName = document.getElementById("paymentMethodSelector").value;

    $.ajax({
        type: "GET",
        url: "/BasicInvoices/IsBankMethod",
        dataType: "text",
        data: {
            paymentMethodName: paymentMethodName
        }
    }).done(function (data) {
        if (data == "true")
            document.getElementById("bankMethodSelector").disabled = false;
        else
            document.getElementById("bankMethodSelector").disabled = true;
    });
}

function CustomAddressSwitched() {
    if (document.getElementById("customCustomerAddressSwitch").checked) {
        document.getElementById("fromList").style.display = "none";
        document.getElementById("customCustomer").style.display = "";

        document.getElementById("CustomSubName").required = true;
        document.getElementById("CustomStreet").required = true;
        document.getElementById("CustomHouseNumber").required = true;
        document.getElementById("CustomZip").required = true;
        document.getElementById("CustomCity").required = true;
    }
    else {
        document.getElementById("fromList").style.display = "";
        document.getElementById("customCustomer").style.display = "none";

        document.getElementById("CustomSubName").required = false;
        document.getElementById("CustomStreet").required = false;
        document.getElementById("CustomHouseNumber").required = false;
        document.getElementById("CustomZip").required = false;
        document.getElementById("CustomCity").required = false;
    }
}

function FillFromItemsList(itemId) {
    $.ajax({
        type: "GET",
        url: "/BasicInvoices/PriceListItems",
        dataType: "json",
        data: {
            searchString: document.getElementById(itemId + "_name").value
        }
    }).done(function (data) {
        document.getElementById(itemId + "_itemSearchResult").innerHTML = "";
        Object.keys(data).forEach(function (key) {
            let item = document.createElement("a");
            item.className = "list-group-item list-group-item-action";
            item.innerHTML = data[key]["Name"];
            item.value = data[key];
            item.onclick = function () {
                document.getElementById(itemId + "_itemSearchResult").innerHTML = "";
                document.getElementById(itemId + "_name").value = data[key]["Name"];
                document.getElementById(itemId + "_price").value = data[key]["Price"];
                document.getElementById(itemId + "_unit").value = data[key]["DefaultUnit"];
                if (isWithVat === 1) {
                    document.getElementById(itemId + "_vat").value = data[key]["Vat"];
                }
                recalculateAmount(itemId);
            };
            document.getElementById(itemId + "_itemSearchResult").appendChild(item);
        });

    });
}