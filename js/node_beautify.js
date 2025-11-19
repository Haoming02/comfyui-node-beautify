import { app } from "../../scripts/app.js";
import { process_graph } from "./functions.js";

function legacy() {
	const menu = document.querySelector(".comfy-menu");

	const beautifyButton = document.createElement("button");
	beautifyButton.textContent = "Beautify";
	beautifyButton.addEventListener("click", () => {
		process_graph();
	});

	const refreshButton = document.getElementById("comfy-refresh-button");
	menu.insertBefore(beautifyButton, refreshButton);
	document.addEventListener("keydown", (e) => {
		if (e.altKey && e.shiftKey && e.code === "KeyF")
			beautifyButton.click();
	});
}

async function frontend() {
	const btn = new (await import("../../scripts/ui/components/button.js")).ComfyButton({
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
		try {
			await frontend();
		} catch {
			// No Frontend
		}

		legacy();
	},
	settings: [
		{
			id: "Beautify.GridSize",
			name: "Grid Size to Round To",
			type: "number",
			defaultValue: 10,
		},
	]
});
