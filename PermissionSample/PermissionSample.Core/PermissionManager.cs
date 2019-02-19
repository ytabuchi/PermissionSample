using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PermissionSample.Core
{
    public static class PermissionManager
    {
        /// <summary>
        /// PermissionPluginを使用してパーミッションをリクエストします。
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns>PermissionとPermissionStatusのDictionary</returns>
        /// <remarks>https://github.com/jamesmontemagno/PermissionsPlugin</remarks>
        public static async Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions)
        {
            var _permissionStatus = new Dictionary<Permission, PermissionStatus>();

            foreach (var permission in permissions)
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                    if (status != PermissionStatus.Granted)
                    {
                        // どのパーミッションで根拠を示す必要があるのか不明
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                        {
                            // ダイアログを表示する必要があるので、リクエストより前に確認する必要がありそう。
                        }
                        // ここでパーミッション確認のダイアログが表示されます。
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(permission))
                            status = results[permission];
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        _permissionStatus.Add(permission, status);
                    }
                    else
                    {
                        _permissionStatus.Add(permission, status);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    _permissionStatus.Clear();
                    _permissionStatus.Add(permission, PermissionStatus.Unknown);
                    return _permissionStatus;
                }
            }

            return _permissionStatus;
        }
    }
}
