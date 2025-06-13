<script>
    function searchTable() {
        var input = document.getElementById("searchInput").value.toLowerCase();
    var rows = document.querySelectorAll("table tbody tr");

    rows.forEach(function (row) {
            var text = row.textContent.toLowerCase();
    if (text.includes(input)) {
        row.style.display = "";
            } else {
        row.style.display = "none";
            }
        });
    }
</script>
