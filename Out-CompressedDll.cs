using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Out_CompressedDll
{
    class Program
    {
        static void Main(string[] args)
        {
            Out_CompressedDll(args[0], args[1]);
        }
        private static void Out_CompressedDll(string FIlePath , string TemplatePath="")
        {
            /*
            .SYNOPSIS

            Creates the C# in-memory version of Out-CompressedDll.ps1. 
            Based entirely off Out-CompressedDll by Matthew Graeber (@mattifestation)
            Original script at https://github.com/PowerShellMafia/PowerSploit/blob/master/ScriptModification/Out-CompressedDll.ps1
                  [Parameter(Mandatory = $True)]
                [String]
                $FilePath,

                [Parameter(Mandatory = $True)]
                [String]
                $TemplatePath
            )

            $Path = Resolve-Path $FilePath

            if (! [IO.File]::Exists($Path))
            {
                Throw "$Path does not exist."
            }

            $FileBytes = [System.IO.File]::ReadAllBytes($Path)

            if (($FileBytes[0..1] | % {[Char]$_}) -join '' -cne 'MZ')
            {
                Throw "$Path is not a valid executable."
            }
             */

            if (File.Exists(FIlePath))
            {
                throw new System.ArgumentException(FIlePath + " does not exist.");
            }

            byte[] FileBytes = File.ReadAllBytes(FIlePath);

            if (FileBytes.Length < 1)
            {
                throw new System.ArgumentException(FIlePath + "is not a valid executable.");
            }

            /*$Length = $FileBytes.Length
            $CompressedStream = New-Object IO.MemoryStream
            $DeflateStream = New - Object IO.Compression.DeflateStream ($CompressedStream, [IO.Compression.CompressionMode]::Compress)
            $DeflateStream.Write($FileBytes, 0, $FileBytes.Length)
            $DeflateStream.Dispose()
            $CompressedFileBytes = $CompressedStream.ToArray()
            $CompressedStream.Dispose()
            $EncodedCompressedFile = [Convert]::ToBase64String($CompressedFileBytes)*/

            Console.WriteLine(Convert.ToBase64String(Compress(FileBytes)));
            /*$Output = @"
            `$EncodedCompressedFile = '$EncodedCompressedFile`'
            `$DeflatedStream = New-Object IO.Compression.DeflateStream([IO.MemoryStream][Convert]::FromBase64String(`$EncodedCompressedFile),[IO.Compression.CompressionMode]::Decompress)
            `$UncompressedFileBytes = New-Object Byte[]($Length)
            `$DeflatedStream.Read(`$UncompressedFileBytes, 0, $Length) | Out-Null
            `$Assembly = [Reflection.Assembly]::Load(`$UncompressedFileBytes)
            `$BindingFlags = [Reflection.BindingFlags] "Public,Static"
            `$a = @()
            `$Assembly.GetType("Costura.AssemblyLoader", `$false).GetMethod("Attach", `$BindingFlags).Invoke(`$Null, @())
            `$Assembly.GetType("Sharphound2.Sharphound").GetMethod("InvokeBloodHound").Invoke(`$Null, @(,`$passed))
        "@

            Get-Content $TemplatePath | %{$_ -replace "#ENCODEDCONTENTHERE", $Output}*/
        }
        private static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
    }
}
