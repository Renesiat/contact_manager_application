let sortAscendingName = true;
let sortAscendingDate = true;
let sortAscendingSalary = true;
function toggleSortByName() {
    const table = document.getElementById("contactsTable");
    const tbody = table.querySelector("tbody");
    const rows = Array.from(tbody.rows);

    const sortedRows = rows.sort((a, b) => {
        const valueA = a.cells[0].textContent.trim().toLowerCase();
        const valueB = b.cells[0].textContent.trim().toLowerCase();
        return sortAscendingName ? valueA.localeCompare(valueB) : valueB.localeCompare(valueA);
    });

    tbody.innerHTML = "";
    sortedRows.forEach(row => tbody.appendChild(row));

    sortAscendingName = !sortAscendingName;

    updateSortIndicator("nameSortIndicator", sortAscendingName);
}

function toggleSortByDateOfBirth() {
    const table = document.getElementById("contactsTable");
    const tbody = table.querySelector("tbody");
    const rows = Array.from(tbody.rows);

    // Сортування рядків за датою
    const sortedRows = rows.sort((a, b) => {
        const dateA = parseDate(a.cells[1].textContent.trim()); // Замість cells[1] виберіть правильну колонку
        const dateB = parseDate(b.cells[1].textContent.trim());
        return sortAscendingDate ? dateA - dateB : dateB - dateA;
    });

    // Перезаписання таблиці
    tbody.innerHTML = "";
    sortedRows.forEach(row => tbody.appendChild(row));

    sortAscendingDate = !sortAscendingDate;

    // Оновлення індикатора сортування (за бажанням)
    updateSortIndicator("dateSortIndicator", sortAscendingDate);
}

function parseDate(dateString) {
    // Перевірка стандартного формату дати
    let parsedDate = Date.parse(dateString);
    if (!isNaN(parsedDate)) {
        return new Date(parsedDate);
    }

    // Перевірка формату "дд.мм.рррр"
    const parts = dateString.split(".");
    if (parts.length === 3) {
        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1; // Місяці починаються з 0
        const year = parseInt(parts[2], 10);
        return new Date(year, month, day);
    }

    // Якщо дата не розпізнана, повертаємо "мінімальну" дату
    return new Date(0);
}

function toggleSortBySalary() {
    const table = document.getElementById("contactsTable");
    const tbody = table.querySelector("tbody");
    const rows = Array.from(tbody.rows);

    const sortedRows = rows.sort((a, b) => {
        const valueA = parseFloat(a.cells[4].textContent.trim()) || 0;
        const valueB = parseFloat(b.cells[4].textContent.trim()) || 0;
        return sortAscendingSalary ? valueA - valueB : valueB - valueA;
    });

    tbody.innerHTML = "";
    sortedRows.forEach(row => tbody.appendChild(row));

    sortAscendingSalary = !sortAscendingSalary;

    updateSortIndicator("salarySortIndicator", sortAscendingSalary);
}
function updateSortIndicator(indicatorId, ascending) {
    const indicator = document.getElementById(indicatorId);
    indicator.textContent = ascending ? "▲" : "▼";
}

function toggleFilterMarried() {
    const checkbox = document.getElementById("filterMarried");
    const rows = Array.from(document.querySelectorAll("#contactsTable tbody tr"));
    const tbody = document.querySelector("#contactsTable tbody");

    const marriedRows = [];
    const unmarriedRows = [];

    rows.forEach(row => {
        const marriedCheckbox = row.querySelector("td:nth-child(3) input[type='checkbox']");
        if (marriedCheckbox && marriedCheckbox.checked) {
            marriedRows.push(row);
        } else {
            unmarriedRows.push(row);
        }
    });

    tbody.innerHTML = ""; 

    if (checkbox.checked) {
        marriedRows.forEach(row => tbody.appendChild(row));
        unmarriedRows.forEach(row => tbody.appendChild(row));
    } else {

        unmarriedRows.forEach(row => tbody.appendChild(row));
        marriedRows.forEach(row => tbody.appendChild(row));
    }
}








