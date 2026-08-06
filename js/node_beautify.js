import { app } from "../../scripts/app.js";
import { process_graph } from "./functions.js";

async function frontend() {
	const btn = new (
		await import("../../scripts/ui/components/button.js")
	).ComfyButton({
		icon: "creation",
		action: () => {
			process_graph();
		},
		tooltip: "Beautify Workflow",
		content: "Beautify",
		classList: "comfyui-button comfyui-menu-mobile-collapse",
	}).element;

	app.menu.actionsGroup.element.after(btn);
}

app.registerExtension({
	name: "Comfy.NodeBeautify",
	async setup() {
		await frontend();
	},
	settings: [
		{
			id: "Beautify.GridSize",
			name: "Grid Size to Round To",
			type: "number",
			defaultValue: 10,
		},
	],
});
