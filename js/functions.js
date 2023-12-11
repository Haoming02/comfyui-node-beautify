import { GRID_SIZE, FIXED_SIZE } from "./configs.js";

function round(value) {
    return value - value % GRID_SIZE;
}

export function Process(graph) {
    graph["nodes"].forEach((node) => {

        if (node["type"] in FIXED_SIZE) {
            node["size"] = FIXED_SIZE[node["type"]];
        }

        ["pos", "size"].forEach((prop) => {
            node[prop][0] = round(node[prop][0]);
            node[prop][1] = round(node[prop][1]);
        });

    });

    graph["groups"].forEach((group) => {

        for (let i = 0; i < 4; i++)
            group["bounding"][i] = round(group["bounding"][i]);

    });

    return graph;
}
