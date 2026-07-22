namespace Adapter.Abstraction.Interfaces;

public interface ISettingAdapter
{
      Task CardFormatConfiguration(
           Guid Guid,
            string Metadata
      );
}