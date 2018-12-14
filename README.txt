AOP realization:

A) Dynamic proxy
Interceptors:
-- Common
	-- MP.WindowsServices.AOP
		-- DynamicProxy
			-- LogMethodExceptionsInterceptor
			-- LogMethodInfoInterceptor

Usage: For ImagesBatchHandlers.PdfImagesBatchHandler
Usage declaration: DependencyResolver.DependencyResolver (L74-76)

B) Code rewriting

Aspects:
-- Common
	-- MP.WindowsServices.AOP
		-- CodeRewriting
			-- LogMethodExceptionsAspect
			-- LogMethodInfoAspect

Usage: In ImagesManager.ImagesBatchProvider
Usage declaration: ImagesBatchProvider.HandlePreviousStepResult