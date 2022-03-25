//bugs: fix calc total amount when remove not last column

let rowCounter = 0;
let sum = 0;

function calcTotalAmount() {
    sum = 0
    for (let i = 0; i < rowCounter; i++) {
        var rowItem = document.getElementById("item" + i);
        const amount = rowItem.childNodes[4].childNodes[0].value;
        sum += parseFloat(amount);
    }
    CalcDiscount();
}

function CalcDiscount() {
    const discountInputAmount = document.getElementById("discountInputAmount").value;
    const discountAmount = document.getElementById("discountAmount");

    if (parseFloat(discountInputAmount) > 0) {
        console.log(discountInputAmount);
        //round to 2 decimal
        const computedDiscount = Math.round(-(sum * (discountInputAmount / 100)) * 100)/100;
        discountAmount.innerHTML = parseFloat(computedDiscount) + ",- Kč";
        const sumWithDiscount = Math.round((sum + computedDiscount) * 100)/100;
        document.getElementById("totalAmount").innerHTML = "Celková cena: " + sumWithDiscount + ",- Kč";
    }
    else {
        discountAmount.innerHTML = "0.0,- Kč";
        document.getElementById("totalAmount").innerHTML = "Celková cena: " + sum + ",- Kč";
    }
}

function DocumentAddItem(isVatPayer, defaultMJ, defaultVat) {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
    const itemId = "item" + rowCounter++;

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
    ItemNameInput.name = "ItemName";
    ItemNameInput.className = "form-control";
    ItemNameInput.required = true;

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    ItemPriceInput.min = 0;
    ItemPriceInput.value = "0.00"
    ItemPriceInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[1].childNodes[0].value;
        const amount = rowItem.childNodes[2].childNodes[0].value;
        rowItem.childNodes[4].childNodes[0].value = price * amount;
        calcTotalAmount();
    }

    const ItemAmountInput = document.createElement("input");
    ItemAmountInput.id = itemId + "_amount";
    ItemAmountInput.name = "ItemAmount";
    ItemAmountInput.className = "form-control";
    ItemAmountInput.type = "number";
    ItemAmountInput.value = 1;
    ItemAmountInput.min = 1;
    ItemAmountInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[1].childNodes[0].value;
        const amount = rowItem.childNodes[2].childNodes[0].value;
        rowItem.childNodes[4].childNodes[0].value = price * amount;
        calcTotalAmount();
    }

    const ItemUnitInput = document.createElement("input");
    ItemUnitInput.name = "ItemUnit";
    ItemUnitInput.value = defaultMJ;
    ItemUnitInput.className = "form-control";

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowCounter;
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

    const ItemPriceInputCol = document.createElement("div");
    ItemPriceInputCol.className = "col";
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
    if (isVatPayer === "True") {
        const ItemPriceWoVat = document.createElement("input");
        ItemPriceWoVat.name = "ItemPriceWoVat";
        ItemPriceWoVat.value = "";
        ItemPriceWoVat.min = 0;
        ItemPriceWoVat.value = "0.00";
        ItemPriceWoVat.type = "number";
        ItemPriceWoVat.className = "form-control";

        const ItemVat = document.createElement("input");
        ItemVat.name = "ItemVat";
        ItemVat.value = defaultVat;
        ItemVat.type = "number";
        ItemVat.className = "form-control";

        const ItemPriceWoVatCol = document.createElement("div");
        ItemPriceWoVatCol.className = "col-md-2";
        ItemPriceWoVatCol.appendChild(ItemPriceWoVat);

        const ItemVatCol = document.createElement("div");
        ItemVatCol.className = "col";
        ItemVatCol.appendChild(ItemVat);

        ItemRow.appendChild(ItemPriceWoVatCol);
        ItemRow.appendChild(ItemVatCol);
    }

    ItemRow.appendChild(ItemTotalAmountCol);
    ItemRow.appendChild(removeButtonCol);
    ItemRow.id = itemId;

    //parent
    const parent = document.getElementById("DocumentItemsBox");
    parent.appendChild(ItemRow);
}


function DocumentAddItemWithValues(name, price, amount, unit) {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
    const itemId = "item" + rowCounter++;

    //items
    const removeButton = document.createElement("button");
    removeButton.innerHTML = "-";
    removeButton.className = "btn btn-outline-danger";
    removeButton.type = "button";
    removeButton.id = itemId;
    removeButton.onclick = function () {
        RemoveItemId(removeButton.id);
    };

    const ItemNameInput = document.createElement("input");
    ItemNameInput.name = "ItemName";
    ItemNameInput.className = "form-control";
    ItemNameInput.required = true;
    ItemNameInput.value = name;

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    ItemPriceInput.min = 0;
    ItemPriceInput.value = price;
    ItemPriceInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[1].childNodes[0].value;
        const amount = rowItem.childNodes[2].childNodes[0].value;
        rowItem.childNodes[4].childNodes[0].value = price * amount;
    }

    const ItemAmountInput = document.createElement("input");
    ItemAmountInput.id = itemId + "_amount";
    ItemAmountInput.name = "ItemAmount";
    ItemAmountInput.className = "form-control";
    ItemAmountInput.type = "number";
    ItemAmountInput.value = amount;
    ItemAmountInput.min = 1;
    ItemAmountInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[1].childNodes[0].value;
        const amount = rowItem.childNodes[2].childNodes[0].value;
        rowItem.childNodes[4].childNodes[0].value = price * amount;
    }

    const ItemUnitInput = document.createElement("input");
    ItemUnitInput.name = "ItemUnit";
    ItemUnitInput.className = "form-control";
    ItemUnitInput.value = unit;

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowCounter;
    ItemTotalAmount.className = "form-control";
    ItemTotalAmount.disabled = true;
    ItemTotalAmount.value = "0.00";

    //cols
    const removeButtonCol = document.createElement("div");
    removeButtonCol.className = "col-md-1";
    removeButtonCol.appendChild(removeButton);

    const ItemNameInputCol = document.createElement("div");
    ItemNameInputCol.className = "col-md-5";
    ItemNameInputCol.appendChild(ItemNameInput);

    const ItemPriceInputCol = document.createElement("div");
    ItemPriceInputCol.className = "col";
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
    ItemRow.appendChild(removeButtonCol);
    ItemRow.appendChild(ItemNameInputCol);
    ItemRow.appendChild(ItemPriceInputCol);
    ItemRow.appendChild(ItemAmountInputCol);
    ItemRow.appendChild(ItemUnitInputCol);
    ItemRow.appendChild(ItemTotalAmountCol);
    ItemRow.id = itemId;

    //parent
    const parent = document.getElementById("DocumentItemsBox");
    parent.appendChild(ItemRow);
}


function RemoveItemId(itemId) {
    if (rowCounter > 1) {
        var rowItem = document.getElementById(itemId);
        rowItem.remove();
        rowCounter--;
        calcTotalAmount();
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
        url: "/Documents/CustomerData",
        dataType: "json",
        data: {
            customerName: customerName
        }

    }).done(function (data) {
        document.getElementById("labelCustomerAddressFirstRow").innerHTML = data["customerStreet"] + data["customerHouseNumber"];
        document.getElementById("labelCustomerAddressSecondRow").innerHTML = data["customerZip"] + "&nbsp;&nbsp;" + data["customerCity"];
        document.getElementById("labelCustomerIco").innerHTML = "IČO: " + data["customerIco"];
        if (data["customerDic"] !== null)
            document.getElementById("labelCustomerDic").innerHTML = "DIČ: " + data["customerDic"];
        else
            document.getElementById("labelCustomerDic").innerHTML = "";
    });
}


function PaymentMethodSelector() {
    var paymentMethodName = document.getElementById("paymentMethodSelector").value;

    $.ajax({
        type: "GET",
        url: "/Documents/IsBankMethod",
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