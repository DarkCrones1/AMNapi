using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Common.Helpers;

public static class StatusDeletedHelper
{
    private const string statusActiveBaseCatalog = "Activo";
    private const string statusInactiveBaseCatalog = "Inactivo";

    public static string GetStatusDeletedEntity(bool? isDeleted)
    {
        if (!isDeleted.HasValue)
            return string.Empty;

        return isDeleted.Value ? statusInactiveBaseCatalog : statusActiveBaseCatalog;
    }
}
