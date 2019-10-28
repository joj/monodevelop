using System;
using System.Linq;
using MonoDevelop.AspNetCore.Scaffolding;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.DotNetCore;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace MonoDevelop.AspNetCore.Commands
{
	enum AspNetCoreCommands
	{
		Scaffold
	}

	class ScaffoldNodeExtension : NodeBuilderExtension
	{
		public override Type CommandHandlerType {
			get { return typeof (ScaffoldCommandHandler); }
		}

		public override bool CanBuildNode (Type dataType)
		{
			return true;
		}
	}

	class ScaffoldCommandHandler : NodeCommandHandler
	{
		[CommandHandler (AspNetCoreCommands.Scaffold)]
		public async void Scaffold ()
		{

			var project = IdeApp.ProjectOperations.CurrentSelectedProject as DotNetProject;
			if (project == null)
				return;

			var folder = CurrentNode.GetParentDataItem (typeof (ProjectFolder), true) as ProjectFolder;
			string parentFolder = folder?.Path ?? project.BaseDirectory;

			var w = new ScaffolderWizard (project, parentFolder);
			//Xwt.Toolkit.NativeEngine.Invoke (() => {
			var res = w.RunWizard ();
			//});

		}

		[CommandUpdateHandler (AspNetCoreCommands.Scaffold)]
		public void ScaffoldUpdate (CommandInfo info)
		{
			var project = CurrentNode.GetParentDataItem (typeof (DotNetProject), true) as DotNetProject;

			info.Enabled = info.Visible = IsAspNetCoreProject (project);
		}

		bool IsAspNetCoreProject (Project project)
		{
			//TODO: this only checks for SDK style project
			return project != null
				&& project.MSBuildProject.GetReferencedSDKs ().Any ();
		}
	}
}
