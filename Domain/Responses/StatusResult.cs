namespace Domain.Responses;

public class StatusResult : ResponseResult
{

}

public class StatusResult<T> : StatusResult
{
    public T? Result { get; set; }
}