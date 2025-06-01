// JavaScript functions for AI-Based Drug Discovery Simulator
// Handles CSV export and file download functionality

window.downloadCsvFile = (filename, csvContent) => {
    // Create a Blob with the CSV content
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    
    // Create a temporary link element
    const link = document.createElement('a');
    
    // Create a URL for the blob
    const url = URL.createObjectURL(blob);
    
    // Set the link attributes
    link.setAttribute('href', url);
    link.setAttribute('download', filename);
    link.style.visibility = 'hidden';
    
    // Add the link to the document
    document.body.appendChild(link);
    
    // Click the link to trigger the download
    link.click();
    
    // Clean up
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};

window.downloadMoleculesAsCsv = (molecules, filename) => {
    if (!molecules || molecules.length === 0) {
        alert('No data to export');
        return;
    }
    
    // Define CSV headers
    const headers = [
        'ID',
        'Formula',
        'Name',
        'SMILES',
        'Molecular Weight',
        'LogP',
        'H-Bond Donors',
        'H-Bond Acceptors',
        'Rotatable Bonds',
        'Polar Surface Area',
        'Drug Likeness Score',
        'Toxicity Score',
        'Solubility Score',
        'Overall Score',
        'Created Date'
    ];
    
    // Create CSV content
    let csvContent = headers.join(',') + '\n';
    
    molecules.forEach(molecule => {
        const row = [
            molecule.id,
            `"${molecule.formula}"`,
            `"${molecule.name}"`,
            `"${molecule.smiles}"`,
            molecule.molecularWeight.toFixed(2),
            molecule.logP.toFixed(2),
            molecule.hBondDonors,
            molecule.hBondAcceptors,
            molecule.rotatableBonds,
            molecule.polarSurfaceArea.toFixed(2),
            molecule.drugLikenessScore.toFixed(2),
            molecule.toxicityScore.toFixed(2),
            molecule.solubilityScore.toFixed(2),
            molecule.overallScore.toFixed(2),
            `"${new Date(molecule.createdAt).toLocaleString()}"`
        ];
        csvContent += row.join(',') + '\n';
    });
    
    // Download the file
    window.downloadCsvFile(filename, csvContent);
};

window.copyToClipboard = async (text) => {
    try {
        await navigator.clipboard.writeText(text);
        return true;
    } catch (err) {
        console.error('Failed to copy text: ', err);
        // Fallback for older browsers
        const textArea = document.createElement('textarea');
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        try {
            document.execCommand('copy');
            document.body.removeChild(textArea);
            return true;
        } catch (fallbackErr) {
            document.body.removeChild(textArea);
            return false;
        }
    }
};

// Chart.js helper functions for statistics visualization
window.createPieChart = (canvasId, data, labels, title) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: [
                    '#FF6384',
                    '#36A2EB',
                    '#FFCE56',
                    '#4BC0C0',
                    '#9966FF',
                    '#FF9F40'
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: title
                }
            }
        }
    });
};

window.createBarChart = (canvasId, data, labels, title, xAxisLabel, yAxisLabel) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: yAxisLabel,
                data: data,
                backgroundColor: '#36A2EB',
                borderColor: '#2E8BC0',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: title
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: xAxisLabel
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: yAxisLabel
                    }
                }
            }
        }
    });
};

window.createHistogram = (canvasId, data, title, xAxisLabel, yAxisLabel) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    
    // Create histogram bins
    const min = Math.min(...data);
    const max = Math.max(...data);
    const binCount = 10;
    const binSize = (max - min) / binCount;
    
    const bins = Array(binCount).fill(0);
    const binLabels = [];
    
    for (let i = 0; i < binCount; i++) {
        const binStart = min + i * binSize;
        const binEnd = min + (i + 1) * binSize;
        binLabels.push(`${binStart.toFixed(1)}-${binEnd.toFixed(1)}`);
    }
    
    data.forEach(value => {
        const binIndex = Math.min(Math.floor((value - min) / binSize), binCount - 1);
        bins[binIndex]++;
    });
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: binLabels,
            datasets: [{
                label: 'Frequency',
                data: bins,
                backgroundColor: '#4BC0C0',
                borderColor: '#36A2EB',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: title
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: xAxisLabel
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: yAxisLabel
                    }
                }
            }
        }
    });
};

// Utility functions for formatting
window.formatNumber = (number, decimals = 2) => {
    return number.toFixed(decimals);
};

window.formatPercentage = (number) => {
    return (number * 100).toFixed(1) + '%';
};

// Bootstrap tooltip initialization
window.initializeTooltips = () => {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};

// Initialize tooltips when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    window.initializeTooltips();
});
