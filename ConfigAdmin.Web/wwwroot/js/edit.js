function insertNewHtml(propertyName) {
    if (propertyName != null) {
        let btnGroup = document.getElementById("btn-group");

        let rowDiv = document.createElement('div');
        rowDiv.classList.add("row", "mb-3");
        rowDiv.innerHTML = `
            <label class="col-sm-2 col-form-label">${propertyName}</label>
            <div class="col-sm-10">
                <input class="form-control" type="text" id="Data_Properties_${propertyName}_" name="Data.Properties[${propertyName}]" value="">
            </div>`;

        btnGroup.parentNode.insertBefore(rowDiv, btnGroup);
    }
}

function addProperty() {

    let serverName = document.getElementById("Data_Name");

    if (serverName.value == "DEFAULTS") {
        let propertyName = prompt("New property name");
        insertNewHtml(propertyName);
    } else {
        modal.show();
    }
}

document.getElementById("btn-new").addEventListener("click", addProperty);
let modal = new bootstrap.Modal(document.getElementById('propertyModal'))

$('#saveChangesBtn').on('click', function () {
    let selectedProperty = $('#newPropertyName').val();
    insertNewHtml(selectedProperty);
    modal.hide();
});