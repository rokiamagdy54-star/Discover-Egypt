INSERT INTO Categories (Title, Description, Name)
VALUES 
('Ancient Egyptian Sites', 'Pharaonic temples, pyramids, and ancient ruins', 'ancient-egypt'),

('Museums', 'Museums showcasing Egyptian history and artifacts', 'museums'),

('Islamic & Coptic Sites', 'Mosques, churches, and religious landmarks', 'religious'),

('Beaches & Coastal Areas', 'Red Sea and Mediterranean beaches', 'beaches'),

('Nile Attractions', 'Nile cruises, river views, and activities', 'nile'),

('Desert & Safari', 'Desert trips, oases, and safari experiences', 'desert'),

('Modern Attractions', 'Malls, towers, and modern entertainment spots', 'modern'),

('Cultural & Heritage', 'Traditional markets, festivals, and local culture', 'culture');

INSERT INTO Company(Name, Email, Phone, Description)
VALUES
(
'Egypt Travel',
'info@egypttravel.com',
'01012345678',
'شركة سياحة متخصصة في الرحلات داخل مصر'
),

(
'Nile Tours',
'nile@tours.com',
'01123456789',
'رحلات نيلية وسياحة فاخرة في مصر'
),

(
'Desert Adventures',
'desert@adventure.com',
'01234567890',
'رحلات سفاري وصحراء في مصر'
);


INSERT INTO ReadyPlans 
(Title, Description, Price, GuideId, CompanyId, StartDateTime, EndDateTime, ImageUrl, Status)
VALUES
(
'Cairo Historical Tour',
'زيارة الأهرامات والمتحف المصري وبرج القاهرة',
500,
1,
1,
GETDATE(),
DATEADD(DAY, 1, GETDATE()),
'/images/licenses/7491a66a-ec8a-40a6-bec5-e38d89ad63a0.png',
1
),

(
'Luxor Temples Tour',
'استكشاف معابد الكرنك ووادي الملوك',
800,
1,
1,
GETDATE(),
DATEADD(DAY, 1, GETDATE()),
'https://images.unsplash.com/photo-1589395937772-f6704c5f5d4b',
1
),

(
'Aswan Nile Trip',
'رحلة نيلية في أسوان وزيارة المعالم',
600,
1,
1,
GETDATE(),
DATEADD(DAY, 1, GETDATE()),
'https://images.unsplash.com/photo-1578926375605-eaf7559b1458',
1
),

(
'Alexandria Sea Tour',
'زيارة البحر والمكتبة وقلعة قايتباي',
550,
1,
1,
GETDATE(),
DATEADD(DAY, 1, GETDATE()),
'https://images.unsplash.com/photo-1578922746465-3a80a228f223',
1
);