using System.Collections.Generic;

namespace Sds.Events.Domain.Core;

public record ResponseResult(string Title, short Status, string[] essages);