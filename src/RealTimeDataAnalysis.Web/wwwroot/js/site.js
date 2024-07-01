let tempChart, humidityChart, combinedChart;
function fetchData() {
    console.log("Fetching data...");
    $.ajax({
        url: 'http://localhost:5001/api/Analysis/lastminute',
        method: 'GET',
        success: function (data) {
            console.log("Data fetched:", data);
            if (data && data.length > 0) {
                var timestamps = data.map(entry => new Date(entry.timestamp).toLocaleTimeString('tr-TR'));
                var temperatures = data.map(entry => entry.averageTemperature);
                var humidities = data.map(entry => entry.averageHumidity);

                console.log("Timestamps:", timestamps);
                console.log("Temperatures:", temperatures);
                console.log("Humidities:", humidities);

                tempChart = updateChart(tempChart, 'temperatureChart', timestamps, temperatures, 'Ortalama Sıcaklık (°C)', 'rgb(255, 99, 132)');
                humidityChart = updateChart(humidityChart, 'humidityChart', timestamps, humidities, 'Ortalama Nem (%)', 'rgb(54, 162, 235)');
                combinedChart = updateNormalizedChart(combinedChart, temperatures, humidities, timestamps);
                updateLastFetchDate();
                console.log("Charts updated");
            } else {
                console.log("No data available");
            }
        },
        error: function (error) {
            console.error('Error fetching data', error);
        }
    });
}

function updateChart(chart, canvasId, labels, data, label, color) {
    if (chart) {
        console.log("Updating existing chart:", label);
        chart.data.labels = labels;
        chart.data.datasets[0].data = data;
        chart.update();
    } else {
        console.log("Creating new chart:", label);
        let ctx = document.getElementById(canvasId).getContext('2d');
        chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: label,
                    data: data,
                    borderColor: color,
                    tension: 0.1
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: false
                    }
                }
            }
        });
    }
    return chart;
}

function updateNormalizedChart(chart, temperatures, humidities, timestamps) {
 

    if (chart) {
        console.log("Updating existing combined chart");
        chart.data.labels = timestamps;
        chart.data.datasets[0].data = temperatures;
        chart.data.datasets[1].data = humidities;
        chart.update();
    } else {
        console.log("Creating new combined chart");
        var ctx = document.getElementById('combinedChart').getContext('2d');
        chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: timestamps,
                datasets: [{
                    label: 'Normalize Sıcaklık',
                    data: temperatures,
                    borderColor: 'rgb(255, 99, 132)',
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                }, {
                    label: 'Normalize Nem',
                    data: humidities,
                    borderColor: 'rgb(54, 162, 235)',
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                }]
            },
            options: {
                responsive: true,
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100,
                        title: {
                            display: true,
                            text: 'Değer'
                        },
                        ticks: {
                            callback: function (value) {
                                return value + '';
                            }
                        }
                    }
                },
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                var label = context.dataset.label || '';
                                if (label) {
                                    label += ': ';
                                }
                                if (context.datasetIndex === 0) {
                                    label += temperatures[context.dataIndex].toFixed(2) + '°C';
                                } else {
                                    label += humidities[context.dataIndex].toFixed(2) + '%';
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });
    }
    return chart;
}

// İlk çağrı
fetchData();

// 60 saniyede bir yenileme
setInterval(fetchData, 6000);


function updateLastFetchDate() {
    var now = new Date();
    var dateString = now.toLocaleString(); // This will use the user's locale settings
    $('#lastFetchDate').text('Last updated: ' + dateString);
}