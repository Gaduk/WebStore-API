namespace Domain.Entities;

public class BasketItem
{
    public int   Id       { get; init; }
    public int   UserName { get; set;  }
    public int   GoodId   { get; set;  }
    public int   Amount   { get; set;  }

    public Good? Good     { get; set;  } = null;
}