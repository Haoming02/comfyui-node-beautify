import { app } from "../../scripts/app.js";
import { Process } from "./functions.js";

app.ui.settings.addSetting({
	id: "beautify.GridSize",
	name: "Beautify Grid Size",
	type: "number",
	defaultValue: 10,
});

app.registerExtension({
	name: "Comfy.NodeBeautify",
	async setup() {
		const menu = document.querySelector(".comfy-menu");

		const beautifyButton = document.createElement("button");
		beautifyButton.textContent = "Beautify";
		beautifyButton.addEventListener("click", () => {
			app.loadGraphData(Process(app.graph.serialize()));
		});

		const refreshButton = document.getElementById("comfy-refresh-button");
		menu.insertBefore(beautifyButton, refreshButton);
	}
});
