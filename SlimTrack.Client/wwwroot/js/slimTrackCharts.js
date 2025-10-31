const WEIGHT_CHART_KEY = "_stWeightChart";
const WAIST_CHART_KEY = "_stWaistChart";
const WEIGHT_COLOR = {
  border: "rgb(59, 130, 246)",
  background: "rgba(59, 130, 246, 0.5)"
};
const WAIST_COLOR = {
  border: "rgb(234, 88, 12)",
  background: "rgba(234, 88, 12, 0.5)"
};

function getContext(canvasId) {
  const canvas = document.getElementById(canvasId);
  if (!canvas) {
    console.warn(`[SlimTrack] Canvas '${canvasId}' not found.`);
    return null;
  }
  return canvas.getContext("2d");
}

function ensureChartJs() {
  if (!window.Chart) {
    console.error("Chart.js is not available. Make sure chart.umd.min.js is loaded.");
    return null;
  }
  return window.Chart;
}

function destroyExistingChart(chartKey) {
  const existing = window[chartKey];
  if (existing) {
    existing.destroy();
  }
}

function createDataset(label, data, colors) {
  return {
    label,
    data,
    tension: 0.3,
    fill: false,
    borderColor: colors.border,
    backgroundColor: colors.background,
    pointRadius: 3,
    pointHoverRadius: 5
  };
}

function buildWeightConfig(labels, data, isHorizontal, unit) {
  const dataset = createDataset(`体重(${unit})`, data, WEIGHT_COLOR);
  if (isHorizontal) {
    return {
      type: "line",
      data: {
        labels,
        datasets: [dataset]
      },
      options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {
            position: "top",
            beginAtZero: false,
            title: {
              display: true,
              text: `体重(${unit})`
            }
          },
          y: {
            ticks: {
              font: { size: 10 }
            }
          }
        },
        plugins: {
          legend: { display: false },
          tooltip: { enabled: true }
        }
      }
    };
  }

  return {
    type: "line",
    data: {
      labels,
      datasets: [dataset]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      scales: {
        y: { beginAtZero: false }
      },
      plugins: {
        legend: { display: true }
      }
    }
  };
}

function buildWaistConfig(labels, data, isHorizontal) {
  const dataset = createDataset("腰围(厘米)", data, WAIST_COLOR);
  if (isHorizontal) {
    return {
      type: "line",
      data: {
        labels,
        datasets: [dataset]
      },
      options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {
            position: "top",
            beginAtZero: false,
            title: {
              display: true,
              text: "腰围(厘米)"
            }
          },
          y: {
            ticks: {
              font: { size: 10 }
            }
          }
        },
        plugins: {
          legend: { display: false },
          tooltip: { enabled: true }
        }
      }
    };
  }

  return {
    type: "line",
    data: {
      labels,
      datasets: [dataset]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      scales: {
        y: { beginAtZero: false }
      },
      plugins: {
        legend: { display: true }
      }
    }
  };
}

export function renderWeightLine(canvasId, labels, data, isHorizontal = false, isJin = true) {
  const ctx = getContext(canvasId);
  const ChartJs = ensureChartJs();
  if (!ctx || !ChartJs) {
    return;
  }

  destroyExistingChart(WEIGHT_CHART_KEY);
  const unit = isJin ? "斤" : "公斤";
  const config = buildWeightConfig(labels, data, isHorizontal, unit);
  window[WEIGHT_CHART_KEY] = new ChartJs(ctx, config);
}

export function renderWaistLine(canvasId, labels, data, isHorizontal = false) {
  const ctx = getContext(canvasId);
  const ChartJs = ensureChartJs();
  if (!ctx || !ChartJs) {
    return;
  }

  destroyExistingChart(WAIST_CHART_KEY);
  const config = buildWaistConfig(labels, data, isHorizontal);
  window[WAIST_CHART_KEY] = new ChartJs(ctx, config);
}
