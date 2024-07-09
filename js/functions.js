import { app } from "../../scripts/app.js";

function round(value, grid_size) {
    return value - value % grid_size;
}

export function Process(graph) {
    const GRID_SIZE = app.ui.settings.getSettingValue("beautify.GridSize", 10);

    graph["nodes"].forEach((node) => {
        ["pos", "size"].forEach((prop) => {
            node[prop][0] = round(node[prop][0], GRID_SIZE);
            node[prop][1] = round(node[prop][1], GRID_SIZE);
        });
    });

    graph["groups"].forEach((group) => {
        for (let i = 0; i < 4; i++)
            group["bounding"][i] = round(group["bounding"][i], GRID_SIZE);
    });

    return graph;
}
