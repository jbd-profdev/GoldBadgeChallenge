using GoldBadgeChallenge.Data.Entities;
using GoldBadgeChallenge.Data.Entities.Enums;
using GoldBadgeChallenge.Repository;

namespace GBRepositoryTests;

public class DeliveryRepositoryTest
{
    private DeliveryRepository _repo;
    private Delivery _delivery42;
    private Delivery _delivery100;
    private Delivery _delivery789;
    private Delivery _delivery1;

    public DeliveryRepositoryTest()
    {
        _repo = new DeliveryRepository();

        _delivery42 = new Delivery(DateOnly.Parse("2023/09/25"),DateOnly.Parse("2023/10/07"),OrderStatus.Canceled,42,35,56);
        _delivery100 = new Delivery(DateOnly.Parse("2023/08/21"),DateOnly.Parse("2023/11/28"),OrderStatus.EnRoute,100,1,3);
        _delivery789 = new Delivery(DateOnly.Parse("2023/09/26"),DateOnly.Parse("2024/06/13"),OrderStatus.Complete,789,3,35);
        _delivery1 = new Delivery(DateOnly.Parse("2023/09/27"), DateOnly.Parse("2023/10/30"), OrderStatus.Canceled, 1, 10, 70);

        _repo.AddDelivery(_delivery42);
        _repo.AddDelivery(_delivery100);
        _repo.AddDelivery(_delivery789);
        _repo.AddDelivery(_delivery1);
    }

    [Fact]
    public void AddDelivery_Bool_Success()
    {
        Delivery delivery = new Delivery();

        bool addResult = _repo.AddDelivery(delivery);

        Assert.True(addResult);
    }

    [Fact]
    public void GetAllDeliveries_CorrectCount()
    {
        int expectedValue = 4;
        int actualValue = _repo.GetAllDeliveries().Count;

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void GetDeliveryByItemNumber_CorrectSelection()
    {
        Delivery order = _repo.GetDeliveryByItemNumber(42);

        Assert.Equal(order, _delivery42);
    }

    [Fact]
    public void GetEnRouteDeliveries_CorrectCount()
    {
        int expectedDelivery = 1;
        int actualDelivery = _repo.GetEnRouteDeliveries().Count;

        Assert.Equal(expectedDelivery, actualDelivery);
    }

    [Fact]
    public void GetCompleteDeliveries_CorrectCount()
    {
        int expectedCount = 1;
        int actualCount = _repo.GetCompleteDeliveries().Count;

        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public void DeleteDelivery_RemovalSuccess()
    {
        bool removeResult = _repo.DeleteDelivery(_delivery789);
        int expectedCount = 3;
        int actualCount = _repo.GetAllDeliveries().Count();

        Assert.True(removeResult);
        Assert.Equal(expectedCount, actualCount);
    }

}