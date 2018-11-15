protected void ExecuteWithExceptionHandling(Action runthis)
{
	try
	{
		runthis();
	}
	catch (Exception e)
	{
		return;
	}
}

protected T ExecuteWithExceptionHandling<T>(Func<T> runthis)
{
	try
	{
		return runthis();
	}
	catch (Exception e)
	{
		return default(T);
	}
}

protected async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> runthis)
{
	try
	{
		return await runthis();
	}
	catch (Exception e)
	{
		return default(T);
	}
}