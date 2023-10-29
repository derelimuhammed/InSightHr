// Dosya girişi değişikliği algılama
document.getElementById('fileInput').addEventListener('change', function (e) {
    var selectedFile = e.target.files[0];
    var uploadedImage = document.getElementById('uploadedImage');

    if (selectedFile) {
        // Seçilen dosya bir resimse, resmi göster
        if (/\.(jpe?g|png|gif|bmp)$/i.test(selectedFile.name)) {
            var reader = new FileReader();

            reader.onload = function (e) {
                uploadedImage.src = e.target.result;
            };

            reader.readAsDataURL(selectedFile);
        } else {
            alert('Lütfen bir resim dosyası seçin.');
        }
    }
});
function exportAllTablesToExcel() {
    const tables = document.getElementsByTagName('table');
    let csvContent = 'data:text/csv;charset=utf-8,';

    for (const table of tables) {
        const rows = table.getElementsByTagName('tr');

        for (const row of rows) {
            const cells = row.getElementsByTagName('td');
            const rowData = [];

            for (const cell of cells) {
                rowData.push(cell.textContent);
            }

            csvContent += rowData.join(',') + '\n';
        }
    }

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement('a');
    link.setAttribute('href', encodedUri);
    link.setAttribute('download', 'data.csv');
    document.body.appendChild(link);

    link.click();
}
// HTML sayfasındaki tüm tabloları Excel dosyası olarak indirme fonksiyonu
function exportAllTablesToExcel() {
    const tables = document.querySelectorAll('table'); // Tüm tabloları seçin

    if (tables.length === 0) {
        alert('Sayfada hiç tablo bulunamadı.');
        return;
    }

    const excelTable = new ExcelJS.Workbook();

    tables.forEach((table, index) => {
        const worksheet = excelTable.addWorksheet(`Sheet ${index + 1}`);
        const rows = table.querySelectorAll('tr');

        rows.forEach((row, rowIndex) => {
            const cells = row.querySelectorAll('td, th'); // td ve th hücrelerini al
            const rowData = [];

            cells.forEach(cell => {
                // Hücre içeriğindeki <a> etiketlerini kontrol et
                const anchor = cell.querySelector('a');
                if (anchor) {
                    rowData.push(anchor.textContent);
                } else {
                    rowData.push(cell.textContent);
                }
            });

            if (rowIndex === 0 && cells.length > 0) {
                worksheet.addRow([]); // Boş bir satır ekleyin
                worksheet.addRow(rowData); // Başlıkları ekleyin
            } else {
                worksheet.addRow(rowData); // Verileri ekleyin
            }
        });
    });

    excelTable.xlsx.writeBuffer().then(data => {
        const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement('a');
        a.href = url;
        a.download = 'all_tables.xlsx';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
    });
}










