const uri = 'api/Lectors'; // наше посилання за яким ми отримаємо список наших об'єктів
let lectors = []; // глобальна змінна для зберігання лекторів

function getLectors() {
    fetch(uri) // звертається до апі, щоб отримати усіх лекторів
        .then(response => {
            if(!response.ok){ // перевіряє чи все ок з запросом
                return response.text().then(text => { throw new Error(text) })} // якщо ні, кидає експешн
            document.getElementById('errorDB').innerHTML = "";
            return response.json();}) // повертає джсон
        .then(data => _displayLectors(data))  // викликає функцію для виведення та збереження лекторів
        .catch(error => document.getElementById('errorDB').innerHTML = error.toString());
}

function addLector() {
    // Отримує дані з інпутів за id 
    const addNameTextbox = document.getElementById('add-FullName');
    const addPhoneTextbox = document.getElementById('add-Phone');
    const addDegreeTextbox = document.getElementById('add-Degree');
    const addOrgsTextbox = document.getElementById('add-Orgs');
    
    // створєю зміну лектора
    const lector = {
        fullName: addNameTextbox.value.trim(),
        phone: addPhoneTextbox.value.trim(),
        degree: addDegreeTextbox.value.trim(),
        OrganizationsIds: addOrgsTextbox.value.replaceAll(" ", "").split(",")
    };
    // метод POST
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(lector)
    })
        .then(response => {
            if(!response.ok){ // перевіряє чи все ок з запросом
                return response.text().then(text => { throw new Error(text) })} // якщо ні, кидає експешн
            document.getElementById('errorDB').innerHTML = "";
            return response.json();}) // повертає джсон
        .then(() => {
            getLectors(); // отримує нових лекторів
            addNameTextbox.value = ''; //очищає комірки інпутів
            addPhoneTextbox.value = '';
            addDegreeTextbox.value = '';
            addOrgsTextbox.value = '';
        })
        .catch(error => document.getElementById('errorDB').innerHTML = error.toString());
}

function deleteLector(id) {
    // видаляє лектора за id і запитує зміни
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getLectors())
        .catch(error => document.getElementById('errorDB').innerHTML = error.toString());
}

function displayEditForm(id) {
    // пошук за id лектора
    const lector = lectors.find(lector => lector.id === id);
    // вставляє дані в форми
    // УВАГА УВАГА
    // ТРЕБА ПИСАТИ lector.phone, а не lector.Phone як зазначено в классах, для перевірки, запустіть гет запрос
    // і подивіться як він повертає (зазвичай, перша літера стає маленькою)
    document.getElementById('edit-Id').value = lector.id;
    document.getElementById('edit-FullName').value = lector.fullName;
    document.getElementById('edit-Phone').value = lector.phone;
    document.getElementById('edit-Degree').value = lector.degree;
    document.getElementById('edit-Orgs').value = lector.organizationsIds;
    document.getElementById('editLector').style.display = 'block';
}

function updateLector() {
    // метод PUT
    const lectorId = document.getElementById('edit-Id').value; // бере ID
    const lector = {
        id: parseInt(lectorId, 10),
        fullName: document.getElementById('edit-FullName').value.trim(), //string.trim() прибирає пробіли з кінців
        phone: document.getElementById('edit-Phone').value.trim(),
        degree: document.getElementById('edit-Phone').value.trim(),
        // користувач вводить id організацій через кому, воно розпарсує цей string за допомогою string.split
        organizationsIds: document.getElementById('edit-Orgs').value.replaceAll(" ", "")
            .split(",")
            .map(str => {
                return Number(str)
            })  
    }
    //передає в контроллер
    fetch(`${uri}/${lectorId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(lector)
    })
        .then(response => {
            if(!response.ok){ // перевіряє чи запрос повернув помилку
                return response.text().then(text => { throw new Error(text) })} // якщо ні, кидає експешн
            document.getElementById('errorDB').innerHTML = "";
            return getLectors();})
        .then(() => getLectors())
        // запрос змін
        .catch(error => document.getElementById('errorDB').innerHTML = error.toString());

    closeInput(); // приховує поле змін

    return false;
}

function closeInput() {
    // приховує елемент з редагуванням лектора
    document.getElementById('editLector').style.display = 'none';
    document.getElementById('errorDB').innerHTML='';
}


function _displayLectors(data) {
    const tBody = document.getElementById('lectors');  // отримує тіло таблиці
    tBody.innerHTML = '';


    const button = document.createElement('button'); // створює шаблон для кнопки

    data.forEach(lector => {  // ітеруємо по лекторам
        // створюємо унікальну кнопку РЕДАГУВАТИ для кожного лектора
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Редагувати';
        // виклик функції на клік, що заповнює редагуючі поля
        editButton.setAttribute('onclick', `displayEditForm(${lector.id})`);  

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Видалити';
        // виклик функції видалення на клік
        deleteButton.setAttribute('onclick', `deleteLector(${lector.id})`);
        
        let tr = tBody.insertRow(); // вставляємо стрічку
        // заповнюємо данними
        //
        // УВАГА УВАГА
        // ТРЕБА ПИСАТИ lector.phone, а не lector.Phone як зазначено в классах, для перевірки, запустіть гет запрос
        // і подивіться як він повертає (зазвичай, перша літера стає маленькою)
        
        let td0  = tr.insertCell(0); //tr.InsertCell(int) вставляє в указану комірку (починаючи з 0) дані
        let textNodeId = document.createTextNode(lector.id); 
        td0.appendChild(textNodeId);
        
        let td1 = tr.insertCell(1); // комірка залежить від вашого <th> в таблиці
        let textNodeFullName = document.createTextNode(lector.fullName);
        td1.appendChild(textNodeFullName);

        let td2 = tr.insertCell(2);
        let textNodePhone = document.createTextNode(lector.phone);
        td2.appendChild(textNodePhone);
        
        let td3 = tr.insertCell(3);
        let textNodeDegree = document.createTextNode(lector.degree);
        td3.appendChild(textNodeDegree);
        
        let td30 = tr.insertCell(4);
        // оскільки у мене для лектора виводятся усі назви організацій, у яких він є, то ми об'єднуємо
        // усі імена через функцію array.join(" ")
        let textNodeOrgs = document.createTextNode(lector.organizations.join(" "));
        td30.appendChild(textNodeOrgs);
        
        let td4 = tr.insertCell(5); //вставляємо кнопку редагування
        td4.appendChild(editButton);

        let td5 = tr.insertCell(6); // вставляємо кнопку видалення
        td5.appendChild(deleteButton);
    });

    lectors = data; // вносимо дані в змінну
}

