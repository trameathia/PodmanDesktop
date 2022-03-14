using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodmanDesktop
{
	public sealed class NotifyTaskComplete<TResult> : ObservableObject
	{
		public Task<TResult> Task { get; private set; }
		public TResult Result => Task.Status == TaskStatus.RanToCompletion ? Task.Result : default;
		public TaskStatus Status => Task.Status;
		public bool IsCompleted => Task.IsCompleted;
		public bool IsNotCompleted => !Task.IsCompleted;
		public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
		public bool IsCanceled => Task.IsCanceled;
		public bool IsFaulted => Task.IsFaulted;

		public AggregateException Exception => Task.Exception;
		public Exception InnerException => Exception?.InnerException;
		public string ErrorMessage => InnerException?.Message;

		public NotifyTaskComplete(Task<TResult> task)
		{
			Task = task;

			if (!task.IsCompleted)
			{
				Task _ = WatchTaskAsync(task);
			}
		}

		private async Task WatchTaskAsync(Task task)
		{
			try
			{
				await task;
			}
			catch
			{
			}

			OnPropertyChanged(nameof(Status));
			OnPropertyChanged(nameof(IsCompleted));
			OnPropertyChanged(nameof(IsNotCompleted));

			if (task.IsCanceled)
			{
				OnPropertyChanged(nameof(IsCanceled));
			}
			else if (task.IsFaulted)
			{
				OnPropertyChanged(nameof(IsFaulted));
				OnPropertyChanged(nameof(Exception));
				OnPropertyChanged(nameof(InnerException));
				OnPropertyChanged(nameof(ErrorMessage));
			}
			else
			{
				OnPropertyChanged(nameof(IsSuccessfullyCompleted));
				OnPropertyChanged(nameof(Result));
			}
		}
	}
}
