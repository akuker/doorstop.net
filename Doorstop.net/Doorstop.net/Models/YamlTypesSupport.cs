using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Doorstop.net.Models
{
  /// <summary>
  /// This class is used by the YamlDotNet engine for converting
  /// 'level' types to/from Yaml strings. 
  /// </summary>
  public class LevelYamlTypeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(Types.Level);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      if (type == typeof(Types.Level))
      {
        Scalar myLevel;
        try
        {
          myLevel = parser.Expect<Scalar>();
        }
        catch (YamlException ex)
        {
          Logger.Warning("An invalid level string was provided");
          throw ex;
        }
        return new Types.Level(myLevel.Value);
      }
      else
      {
        throw new NotImplementedException("Got type " + type.ToString() + " to deserialize");
      }
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      Types.Level item = value as Types.Level;
      if (item != null)
      {
        emitter.Emit(new Scalar(item.ToString()));
      }
    }
  }

}
