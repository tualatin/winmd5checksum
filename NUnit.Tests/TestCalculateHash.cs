using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Org.Vs.WinMd5.Controllers;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.Interfaces;

namespace NUnit.Tests
{
  [TestFixture]
  public class TestCalculateHash
  {
    private ICalculateHash _calculateHashController;

    [SetUp]
    protected void SetUp() => _calculateHashController = new CalculateHash();

    [Test]
    public async Task TestCalculateHashInParallelAsync()
    {
      ObservableCollection<WinMdChecksumData> collection = new ObservableCollection<WinMdChecksumData>();
      await _calculateHashController.StartCalculationAsync(collection, new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).ConfigureAwait(false);
    }
  }
}
