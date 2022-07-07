function openTab(evt, tableName) {
	// Declare all variables
	var i, tabcontent, tablinks;

	// Get all elements with class="tabcontent" and hide them
	tabcontent = document.getElementsByClassName("tabcontent");
	for (i = 0; i < tabcontent.length; i++) {
		tabcontent[i].style.display = "none";

	}

	// Get all elements with class="tablinks" and remove the class "active"
	tablinks = document.getElementsByClassName("tablinks");
	for (i = 0; i < tablinks.length; i++) {
		tablinks[i].className = tablinks[i].className.replace(" active", "");
	}

	// Show the current tab, and add an "active" class to the button that opened the tab
	document.getElementById(tableName).style.display = "block";
	evt.currentTarget.className += " active";
}


function ExportToExcel(type, tablename, fn, dl) {
	var elt = document.getElementById(tablename);
	var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
	return dl ?
		XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
		XLSX.writeFile(wb, fn || ('Backlog_' + tablename + '.' + (type || 'xlsx')));
}

function callChangefunc(val) {
	window.location.href = "/AMSDefectBacklog?gname=" + val;
}


	
	