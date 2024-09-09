import { app } from "../../scripts/app.js";
import { Process } from "./functions.js";

app.ui.settings.addSetting({
	id: "beautify.GridSize",
	name: "Beautify Grid Size",
	type: "number",
	defaultValue: 10,
});

function legacy() {
	const menu = document.querySelector(".comfy-menu");

	const beautifyButton = document.createElement("button");
	beautifyButton.textContent = "Beautify";
	beautifyButton.addEventListener("click", () => {
		app.loadGraphData(Process(app.graph.serialize()));
	});

	const refreshButton = document.getElementById("comfy-refresh-button");
	menu.insertBefore(beautifyButton, refreshButton);
}

async function frontend() {
	const btn = new (await import("../../scripts/ui/components/button.js")).ComfyButton({
		icon: "creation",
		action: () => {
			app.loadGraphData(Process(app.graph.serialize()));
		},
		tooltip: "Beautify Workflow",
		content: "Beautify",
		classList: "comfyui-button comfyui-menu-mobile-collapse"
	}).element;

	app.menu?.actionsGroup.element.after(btn);
}

app.registerExtension({
	name: "Comfy.NodeBeautify",
	async setup() {

		try {
			await frontend();
		} catch {
			// No Frontend?
		}

		legacy();

	}
});
