let month = 0;
let year = 0;
let rooms = JSON.parse(document.getElementById("rooms").value);

window.onload = function () {
	let main = document.getElementsByTagName("main")[0];
	main.appendChild(loadTable());
	cleanup();
	events();
}

// Cleans up hidden input fields with backend data.
function cleanup() {
	document.getElementById("rooms").remove();
}

// Adds event listeners to elements.
function events() {
	// document.getElementById("month-selector").onchange = selectorUpdate();
}

function loadTable() {
	let table = document.createElement("table");
	table.id = "rooms-table";
	table.classList = "table";
	table.appendChild(loadTableHead());
	table.appendChild(loadTableBody());
	return table;
}

function reloadTable() {
	let table = document.getElementById("rooms-table");
	table.replaceWith(loadTable());
}

function loadTableHead() {
	let tableHead = document.createElement("thead");
	let tableHeadRow = document.createElement("tr");

	tableHeadRow.appendChild(loadTableHeadCell("Actions"));
	tableHeadRow.appendChild(loadTableHeadCell("Type"));
	tableHeadRow.appendChild(loadTableHeadCell("Number"));
	loadReservationHead().forEach(day => { tableHeadRow.appendChild(day) });

	tableHead.appendChild(tableHeadRow);
	return tableHead;
}

function loadReservationHead() {
	let days = [];
	for (let i = 1; i <= getDaysInMonth(getMonthYear().month, getMonthYear().year); i++) {
        let day = document.createElement("th");
		day.classList.add("text-center", "date-header");
		day.innerHTML = i;
		days.push(day);
    }

	return days;
}

function loadTableHeadCell(name) {
	let tableHeadCell = document.createElement("th");
	tableHeadCell.innerHTML = name;
	return tableHeadCell;
}

function getMonthYear() {
	if (month === 0 || year === 0) {
		let date = new Date();
		month = date.getMonth() + 1;
		year = date.getFullYear();
	}
	return { month: month, year: year };
}

function monthSelector() {
	let selector = document.createElement("select");
	selector.type = "month";
	selector.id = "month-selector";
	selector.classList.add("form-select-sm");
	// TODO - selector.onchange = reloadTable();
	let date = new Date();
	for (let i = 0; i < 12; i++) {
		let option = document.createElement("option");
		option.selected = i === 0;
		option.value = date.getFullYear() + "-" + (date.getMonth() + 1);
		option.innerHTML = date.toLocaleString('default', { month: 'long' });
		selector.appendChild(option);
		date.setMonth(date.getMonth() + 1);
	}

	return selector;
}

function selectorUpdate() {
	console.log("Selector update");
	let selector = document.getElementById("month-selector");
	let index = selector.selectedIndex;
	let date = new Date(selector.options[index]);
	month = date.getMonth();
	year = date.getFullYear();
	reloadTable();
}

function loadTableBody() {
	let tableBody = document.createElement("tbody");

	rooms.forEach(room => {
		let row = document.createElement("tr");
		row.appendChild(loadRoomActions(room));
		row.appendChild(loadRoomType(room));
		row.appendChild(loadRoomNumber(room));
		loadReservations(room).forEach(reservation => { row.appendChild(reservation) });
		tableBody.appendChild(row);
	});

	return tableBody;
}

function getDaysInMonth(month, year) {
	const isLeapYear = year % 4 === 0 && year % 100 !== 0 || year % 400 === 0;

	switch (month) {
		case 1:
		case 3:
		case 5:
		case 7:
		case 8:
		case 10:
		case 12:
			return 31;
		case 4:
		case 6:
		case 9:
		case 11:
			return 30;
		case 2:
			return isLeapYear ? 29 : 28;
		default:
			throw new Error("Invalid month");
	}
}

function loadRoomNumber(room) {
	let element = document.createElement("td");

	element.innerHTML = room.Number;

	return element;
}

function loadRoomType(room) {
	let element = document.createElement("td");

	element.innerHTML = roomTypeToString(room.Type);

	return element;
}

function roomTypeToString(type) {
	switch (type) {
		case 0:
			return "None";
		case 1:
			return "Apartment";
		case 2:
			return "Room";
	}
}

function loadRoomActions(room) {
	let element = document.createElement("td");

	element.classList.add("btn-group");
	element.appendChild(addReservatioButton(room, element));
	element.appendChild(deleteRoomButton(room, element));

	return element;
}

function addReservatioButton(room) {
	let btn = document.createElement("a");

	btn.classList.add("btn", "btn-success");
	btn.href = "/Rooms/AddReservation/" + room.Number;
	btn.appendChild(addIcon());

	return btn;
}

function addIcon() {
	let icon = document.createElement("i");
	icon.classList.add("bi", "bi-plus-circle-fill");
	return icon;
}

function deleteRoomButton(room) {
	let btn = document.createElement("a");

	btn.classList.add("btn", "btn-danger");
	btn.href = "/Rooms/Delete/" + room.Number;
	btn.appendChild(deleteIcon());

	return btn;
}

function deleteIcon() {
	let icon = document.createElement("i");
	icon.classList.add("bi", "bi-trash-fill");
	return icon;
}

function loadReservations(room) {
	let reservations = filterReservation(room.Reservations);
	let days = getDaysInMonth(getMonthYear().month, getMonthYear().year);
	let row = [];

	for (let i = 1; i <= days; i++) {
		let currentDate = new Date(getMonthYear().year, getMonthYear().month - 1, i);
		let checkInReservation = getCheckIn(reservations, currentDate);
		let checkOutReservation = getCheckOut(reservations, currentDate);
		let occupiedReservation = getOccupied(reservations, currentDate);
		let fullCell = document.createElement("td");
		fullCell.classList.add("cell", "text-center");

		if (checkInReservation !== null && checkOutReservation !== null) {
			addCellCheckInCheckOut(checkInReservation, checkOutReservation).forEach(cell => fullCell.appendChild(cell));
		}
		else if (checkOutReservation !== null && checkInReservation === null) {
			addCellCheckOut(checkOutReservation).forEach(cell => fullCell.appendChild(cell));
		}
		else if (checkInReservation !== null && checkOutReservation === null) {
			addCellCheckIn(checkInReservation).forEach(cell => fullCell.appendChild(cell));
		}
		else if (occupiedReservation !== null) {
			fullCell.appendChild(addCellOccupied(occupiedReservation));
		}
		else if (occupiedReservation === null) {
			fullCell.appendChild(addCellEmpty());
		}
		row.push(fullCell);
	}
	return row;
}

function getLink(reservarion) {
	let link = document.createElement("a");
	link.classList = "fill-cell";
	link.href = "/Reservations/Edit/" + reservarion.Id;
	return link;
}

function dateToDays(date) {
	return Math.floor(new Date(date).getTime() / (1000 * 60 * 60 * 24));
}

function getOccupied(reservations, date) {
	let dateOnly = dateToDays(date);

	for (let i = 0; i < reservations.length; i++) {
		let checkIn = dateToDays(reservations[i].CheckIn);
		let checkOut = dateToDays(reservations[i].CheckOut);

		if (checkIn <= dateOnly && checkOut >= dateOnly) {

			return reservations[i];
		}
	}

	return null;
}

function getCheckOut(reservations, date) {
	let dateOnly = dateToDays(date);

	for (let i = 0; i < reservations.length; i++) {
		let checkOut = dateToDays(reservations[i].CheckOut);

		if (checkOut === dateOnly) {

			return reservations[i];
		}
	}

	return null;
}

function getCheckIn(reservations, date) {
	let dateOnly = dateToDays(date);

	for (let i = 0; i < reservations.length; i++) {
		let checkIn = dateToDays(reservations[i].CheckIn);

		if (checkIn === dateOnly) {

			return reservations[i];
		}
	}

	return null;
}

function filterReservation(reservations) {
	var filtered = reservations.filter(r => {
		let beginOfMonth = dateToDays(new Date(getMonthYear().year, getMonthYear().month - 1, 1));
		let endOfMonth = dateToDays(new Date(getMonthYear().year, getMonthYear().month - 1, getDaysInMonth(getMonthYear().month)));
		let checkIn = dateToDays(r.CheckIn);
		let checkOut = dateToDays(r.CheckOut);
		let result = checkIn <= endOfMonth && checkOut >= beginOfMonth;
        console.log("checkin: " + checkIn + "\ncheckout: " + checkOut + "\nbm: " + beginOfMonth + "\nem: " + endOfMonth + "\nrci: " + r.CheckIn + "\nrco: " + r.CheckOut + "\nr: " + result);
		return result;
	});
	return filtered;
}

function addCellEmpty() {
	let cell = document.createElement("div");
	cell.classList.add("cell-empty");
	return cell;
}

function addCellOccupied(reservation) {
	let cell = document.createElement("div");
	cell.classList.add("occupied", "fill-cell");
	cell.appendChild(getLink(reservation));
	return cell;
}

function addCellCheckIn(reservation) {
	let lCell = document.createElement("div");
	let rCell = document.createElement("div");

	lCell.classList.add("cell-half-left");
	rCell.classList.add("cell-half-right", "check-in");
	rCell.appendChild(getLink(reservation));

	return [lCell, rCell];
}

function addCellCheckOut(reservation) {
	let lCell = document.createElement("div");
	let rCell = document.createElement("div");

	lCell.classList.add("cell-half-left", "check-out");
	rCell.classList.add("cell-half-right");

	lCell.appendChild(getLink(reservation));
	return [lCell, rCell];
}

function addCellCheckInCheckOut(checkinReservation, checkoutReservation) {
	let lCell = document.createElement("div");
	let rCell = document.createElement("div");

	lCell.classList.add("cell-half-left", "check-out");
	rCell.classList.add("cell-half-right", "check-in");

	lCell.appendChild(getLink(checkoutReservation));
	rCell.appendChild(getLink(checkinReservation));

	return [lCell, rCell];
}
