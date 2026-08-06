import { app } from "../../scripts/app.js";

/** @param {number} value @param {number} size @returns {number} */
function round(value, size) {
    return Math.round(value / size) * size;
}

export function process_graph() {
    const GRID_SIZE = app.extensionManager.setting.get("Beautify.GridSize");

    const nodes = app.graph._nodes;
    const groups = app.graph._groups;

    groups?.forEach((group) => {
        group.recomputeInsideNodes();
    });

    nodes?.forEach((node) => {
        const [x, y] = node.pos;
        const [w, h] = node.size;

        node.pos[0] = round(x, GRID_SIZE);
        node.pos[1] = round(y, GRID_SIZE);

        node.size[0] = round(w, GRID_SIZE);
        node.size[1] = round(h, GRID_SIZE);
    });

    if (nodes?.length) {
        const minX = Math.min(...nodes.map((n) => n.pos[0]));
        const minY = Math.min(...nodes.map((n) => n.pos[1]));

        nodes.forEach((node) => {
            node.pos[0] -= minX;
            node.pos[1] -= minY;
        });
    }

    groups?.forEach((group) => {
        const _nodes = group._nodes;
        if (!_nodes?.length) return;

        const titleOffset = 60;

        let minX = Infinity,
            minY = Infinity;
        let maxX = -Infinity,
            maxY = -Infinity;

        for (const node of _nodes) {
            const [nx, ny] = node.pos;
            const [nw, nh] = node.size;

            minX = Math.min(minX, nx);
            minY = Math.min(minY, ny - titleOffset);
            maxX = Math.max(maxX, nx + nw);
            maxY = Math.max(maxY, ny + nh);
        }

        group.pos[0] = round(minX - GRID_SIZE * 2, GRID_SIZE);
        group.pos[1] = round(minY - GRID_SIZE * 2, GRID_SIZE);

        group.size[0] = round(maxX - minX + GRID_SIZE * 4, GRID_SIZE);
        group.size[1] = round(maxY - minY + GRID_SIZE * 4, GRID_SIZE);

        group.recomputeInsideNodes();
    });

    app.graph.change();
}
