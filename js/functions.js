import { GRID_SIZE, FIXED_SIZE } from "./configs.js";

function round(value) {
    return value - value % GRID_SIZE;
}

export function Process(graph) {
    graph["nodes"].forEach((node) => {

        if (node["type"] in FIXED_SIZE) {
            node["size"] = FIXED_SIZE[node["type"]];
        }

        ["pos", "size"].forEach((mode) => {
            [0, 1].forEach((index) => {
                node[mode][index] = round(node[mode][index]);
            });
        });

    });

    graph["groups"].forEach((group) => {

        for (let i = 0; i < 4; i++)
            group["bounding"][i] = round(group["bounding"][i]);

    });

    return graph;
}
