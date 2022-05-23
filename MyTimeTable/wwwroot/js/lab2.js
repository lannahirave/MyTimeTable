const uri = 'api/Lectors';
let lectors = [];

function getLectors() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayLectors(data))
        .catch(error => console.error('Unable to get lectors.', error));
}

function addLector() {
    const addNameTextbox = document.getElementById('add-FullName');
    const addPhoneTextbox = document.getElementById('add-Phone');
    const addDegreeTextbox = document.getElementById('add-Degree');
    const addOrgsTextbox = document.getElementById('add-Orgs');
    

    const lector = {
        fullName: addNameTextbox.value.trim(),
        phone: addPhoneTextbox.value.trim(),
        degree: addDegreeTextbox.value.trim(),
        OrganizationsIds: addOrgsTextbox.value.replaceAll(" ", "").split(",")
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(lector)
    })
        .then(response => response.json())
        .then(() => {
            getLectors();
            addNameTextbox.value = '';
            addPhoneTextbox.value = '';
            addDegreeTextbox.value = '';
            addOrgsTextbox.value = '';
        })
        .catch(error => console.error('Unable to add lector.', error));
}

function deleteLector(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getLectors())
        .catch(error => console.error('Unable to delete lector.', error));
}

function displayEditForm(id) {
    const lector = lectors.find(lector => lector.id === id);
    document.getElementById('edit-Id').value = lector.id;
    document.getElementById('edit-FullName').value = lector.fullName;
    document.getElementById('edit-Phone').value = lector.phone;
    document.getElementById('edit-Degree').value = lector.degree;
    document.getElementById('edit-Orgs').value = lector.organizationsIds;
    document.getElementById('editLector').style.display = 'block';
}

function updateLector() {
    const lectorId = document.getElementById('edit-Id').value;
    const lector = {
        id: parseInt(lectorId, 10),
        fullName: document.getElementById('edit-FullName').value.trim(),
        phone: document.getElementById('edit-Phone').value.trim(),
        degree: document.getElementById('edit-Phone').value.trim(),
        organizationsIds: document.getElementById('edit-Orgs').value.replaceAll(" ", "")
            .split(",")
            .map(str => {
                return Number(str)
            })  
    }
        //console.log(document.getElementById('edit-Orgs').value.replaceAll(" ", "")
        //.split(", "));
    //console.log(lector.organizationsIds);

    fetch(`${uri}/${lectorId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(lector)
    })
        .then(() => getLectors())
        .catch(error => console.error('Unable to update lector.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editLector').style.display = 'none';
}


function _displayLectors(data) {
    const tBody = document.getElementById('lectors');
    tBody.innerHTML = '';


    const button = document.createElement('button');

    data.forEach(lector => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Редагувати';
        editButton.setAttribute('onclick', `displayEditForm(${lector.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Видалити';
        deleteButton.setAttribute('onclick', `deleteLector(${lector.id})`);
        
        let tr = tBody.insertRow();

        let td0  = tr.insertCell(0);
        let textNodeId = document.createTextNode(lector.id);
        td0.appendChild(textNodeId);
        
        let td1 = tr.insertCell(1);
        let textNodeFullName = document.createTextNode(lector.fullName);
        td1.appendChild(textNodeFullName);

        let td2 = tr.insertCell(2);
        let textNodePhone = document.createTextNode(lector.phone);
        td2.appendChild(textNodePhone);
        
        let td3 = tr.insertCell(3);
        let textNodeDegree = document.createTextNode(lector.degree);
        td3.appendChild(textNodeDegree);
        
        let td30 = tr.insertCell(4);
        let textNodeOrgs = document.createTextNode(lector.organizations.join(" "));
        td30.appendChild(textNodeOrgs);
        
        let td4 = tr.insertCell(5);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(6);
        td5.appendChild(deleteButton);
    });

    lectors = data;
}
