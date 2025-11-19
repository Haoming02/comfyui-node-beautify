import { app } from "../../scripts/app.js";

/** @param {number} value @param {number} size @returns {number} */
function round(value, size) {
    return Math.round(value / size) * size;
}

export function process_graph() {
    const GRID_SIZE = app.extensionManager.setting.get("Beautify.GridSize");

    app.graph._nodes?.forEach((node) => {
        const [x, y] = node.pos;
        const [w, h] = node.size;

        node.pos[0] = round(x, GRID_SIZE);
        node.pos[1] = round(y, GRID_SIZE);

        node.size[0] = round(w, GRID_SIZE);
        node.size[1] = round(h, GRID_SIZE);
    });

    app.graph._groups?.forEach((group) => {
        const [x, y] = group._pos;
        const [w, h] = group._size;

        group.pos[0] = round(x, GRID_SIZE);
        group.pos[1] = round(y, GRID_SIZE);

        group.size[0] = round(w, GRID_SIZE);
        group.size[1] = round(h, GRID_SIZE);

        group.recomputeInsideNodes();
    });

    app.graph.change();
}
