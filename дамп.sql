-- Вставка объявлений с существующими SubCategoryId
INSERT INTO Listings 
(ListingTypeId, ListingStatusId, SubCategoryId, TxtTitle, MemDescription, DblPrice, IsForSale, IsForRent, DtCreated, IntShow, CategoryId, Rooms, Floor, TotalArea, LandArea, Status)
VALUES
(1, 1, 1, 'Уютная однокомнатная квартира в центре', 'Светлая квартира с хорошим ремонтом.', 5000000, 1, 0, GETDATE(), 1, 10, 1, 3, 35.5, NULL, 'Активно'),

(1, 1, 2, 'Просторная двухкомнатная квартира с балконом', 'Отличное расположение, рядом парк.', 7500000, 1, 0, GETDATE(), 1, 10, 2, 5, 55.0, NULL, 'Активно'),

(1, 1, 6, 'Прекрасный дом в Подмосковье', 'Большой дом с участком и гаражом.', 12000000, 1, 0, GETDATE(), 1, 20, 5, 1, 120.0, 500, 'Активно');

-- Геоданные
INSERT INTO ListingGeo (ListingId, CountryId, RegionId, CityId, TxtAddress, DblLatitude, DblLongitude, IntGoogleMap)
VALUES
((SELECT ListingId FROM Listings WHERE TxtTitle = 'Уютная однокомнатная квартира в центре'), 1, 10, 100, 'г. Москва, ул. Тверская, д.1', 55.7558, 37.6173, 1),

((SELECT ListingId FROM Listings WHERE TxtTitle = 'Просторная двухкомнатная квартира с балконом'), 1, 10, 100, 'г. Москва, ул. Арбат, д.12', 55.7520, 37.5764, 1),

((SELECT ListingId FROM Listings WHERE TxtTitle = 'Прекрасный дом в Подмосковье'), 1, 10, 100, 'Московская область, Истра, ул. Лесная, д.5', 55.9110, 36.8642, 1);

-- Изображения
INSERT INTO ListingImages (ListingId, TxtImage, TxtImageTitle, TxtImageUrl, IntSort)
VALUES
((SELECT ListingId FROM Listings WHERE TxtTitle = 'Уютная однокомнатная квартира в центре'), 'image1.jpg', 'Фото квартиры', '/images/listings/image1.jpg', 1),

((SELECT ListingId FROM Listings WHERE TxtTitle = 'Просторная двухкомнатная квартира с балконом'), 'image2.jpg', 'Фото с балкона', '/images/listings/image2.jpg', 1),

((SELECT ListingId FROM Listings WHERE TxtTitle = 'Прекрасный дом в Подмосковье'), 'image3.jpg', 'Фасад дома', '/images/listings/image3.jpg', 1);
