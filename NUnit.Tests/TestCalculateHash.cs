using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using NUnit.Framework;
using Org.Vs.WinMd5.Controllers;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Data;

namespace NUnit.Tests
{
  [TestFixture]
  public class TestCalculateHash
  {
    private TestContext _currenTestContext;
    private ICalculateHash _calculateHashController;
    private string _testFile1;
    private string _testFile2;

    [SetUp]
    protected void SetUp()
    {
      SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

      _currenTestContext = TestContext.CurrentContext;
      _testFile1 = _currenTestContext.TestDirectory + @"\Files\test1.txt";
      _testFile2 = _currenTestContext.TestDirectory + @"\Files\test2.txt";

      _calculateHashController = new CalculateHash();
    }

    [Test]
    public async Task TestCalculateHashInParallelAsync()
    {
      var stopwatch = new Stopwatch();
      var collection = new ObservableCollection<WinMdChecksumData>
      {
        new WinMdChecksumData
        {
          FileName = _testFile1,
          Sha1IsEnabled = true,
          Sha256IsEnabled = true,
          Sha512IsEnabled = true
        },
        new WinMdChecksumData
        {
          FileName = _testFile2,
          Sha1IsEnabled = false,
          Sha256IsEnabled = true
        }
      };

      stopwatch.Start();
      var test = await _calculateHashController.StartCalculationAsync(collection, new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).ConfigureAwait(false);
      stopwatch.Stop();
    }
  }
}
