function KillChildren
{
    Param(
        [Parameter(Mandatory=$True,Position=1)]
            [int]$parentProcessId
    )
echo $parentProcessId
    Get-WmiObject win32_process | where {$_.ParentProcessId -eq $parentProcessId} | ForEach { KillChildren $_.ProcessId }
    Get-WmiObject win32_process | where {$_.ParentProcessId -eq $parentProcessId} | ForEach { Stop-Process $_.ProcessId 2>$null }
}