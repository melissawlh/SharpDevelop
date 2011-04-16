﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.PackageManagement;
using ICSharpCode.PackageManagement.Design;
using NuGet;

namespace PackageManagement.Tests.Helpers
{
	public class TestablePackageViewModel : PackageViewModel
	{
		public FakePackageRepository FakeSourcePackageRepository;
		public FakePackageManagementService	FakePackageManagementService;
		public FakePackageManagementEvents FakePackageManagementEvents;
		public FakePackage FakePackage;
		public FakeLogger FakeLogger;
		
		public TestablePackageViewModel(FakePackageManagementService packageManagementService)
			: this(
				new FakePackage(),
				new FakePackageRepository(),
				packageManagementService,
				new FakePackageManagementEvents(),
				new FakeLogger())
		{		
		}
		
		public TestablePackageViewModel(
			FakePackage package,
			FakePackageRepository sourceRepository,
			FakePackageManagementService packageManagementService,
			FakePackageManagementEvents packageManagementEvents,
			FakeLogger logger)
			: base(
				package,
				sourceRepository,
				packageManagementService,
				packageManagementEvents,
				logger)
		{
			this.FakePackage = package;
			this.FakePackageManagementService = packageManagementService;
			this.FakePackageManagementEvents = packageManagementEvents;
			this.FakeSourcePackageRepository = sourceRepository;
			this.FakeLogger = logger;
		}
		
		protected override PackageViewModelOperationLogger CreateLogger(ILogger logger)
		{
			PackageViewModelOperationLogger operationLogger = base.CreateLogger(logger);
			operationLogger.AddingPackageMessageFormat = "Installing...{0}";
			operationLogger.RemovingPackageMessageFormat = "Uninstalling...{0}";
			OperationLoggerCreated = operationLogger;
			return operationLogger;
		}
		
		public PackageViewModelOperationLogger OperationLoggerCreated;
		
		public PackageOperation AddOneFakeInstallPackageOperationForViewModelPackage()
		{
			var operation = new PackageOperation(FakePackage, PackageAction.Install);
			
			FakePackageManagementService
				.FakePackageManagerToReturnFromCreatePackageManager
				.PackageOperationsToReturnFromGetInstallPackageOperations
				.Add(operation);
			
			return operation;
		}
		
		public PackageOperation AddOneFakeUninstallPackageOperation()
		{
			var package = new FakePackage("PackageToUninstall");			
			var operation = new PackageOperation(package, PackageAction.Uninstall);
			
			FakePackageManagementService
				.FakePackageManagerToReturnFromCreatePackageManager
				.PackageOperationsToReturnFromGetInstallPackageOperations
				.Add(operation);
			
			return operation;
		}
	}
}
