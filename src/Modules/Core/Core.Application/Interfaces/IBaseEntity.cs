namespace Core.Application.Interfaces;

public interface IBaseEntity
{
      int id {set; }
      Guid guid {set; }
      DateTime created_at {set;}
      DateTime updated_at {set;}
      bool is_active {set;}
      bool is_default {get;} 
      short? external_id {get;}
}
