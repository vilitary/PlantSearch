CREATE TABLE account (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    email VARCHAR(100),
	role VARCHAR(20) DEFAULT 'user',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE plant (
    id SERIAL PRIMARY KEY,
    plant_name VARCHAR(100) NOT NULL,
	science_name VARCHAR(100) NOT NULL,
	origin VARCHAR(100) NOT NULL,
	growth_duration INTEGER, 
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE plantimages (
    id SERIAL PRIMARY KEY,
    plant_id INTEGER REFERENCES plant(id) ON DELETE CASCADE,
    image_url TEXT NOT NULL
);

CREATE TABLE plant_type (
    id SERIAL PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE season (
    id SERIAL PRIMARY KEY,
    season_name VARCHAR(50) NOT NULL UNIQUE,  -- Ví dụ: Xuân, Hạ, Thu, Đông
    start_month INTEGER CHECK (start_month BETWEEN 1 AND 12),
    end_month INTEGER CHECK (end_month BETWEEN 1 AND 12)
);

CREATE TABLE fertilizer_type (
    id SERIAL PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE,   -- Ví dụ: Hữu cơ, Vô cơ, NPK
    description TEXT
);

CREATE TABLE soil_type (
    id SERIAL PRIMARY KEY,
    soil_name VARCHAR(100) NOT NULL UNIQUE,   -- Ví dụ: đất cát, đất thịt, đất phù sa
    description TEXT,
	notes TEXT
);

CREATE TABLE plant_soil (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    soil_id INTEGER NOT NULL REFERENCES soil_type(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, soil_id)
);

CREATE TABLE disease_type (
    id SERIAL PRIMARY KEY,
    disease_name VARCHAR(100) NOT NULL UNIQUE,
    symptoms TEXT,
    causes TEXT
);

CREATE TABLE diseasetreatment (
    id SERIAL PRIMARY KEY,
    disease_id INTEGER REFERENCES disease_type(id) ON DELETE CASCADE,
    treatment_method TEXT NOT NULL,
    notes TEXT
);

CREATE TABLE suitable_region (
    id SERIAL PRIMARY KEY,
    region_name VARCHAR(100) NOT NULL UNIQUE,
    climate_type VARCHAR(100),         -- Ví dụ: nhiệt đới, ôn đới               
    notes TEXT
);

CREATE TABLE cultivation_method (
    id SERIAL PRIMARY KEY,
    method_name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,                  -- Mô tả chi tiết cách trồng
    notes TEXT
);

CREATE TABLE plant_fertilizer (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    fertilizer_id INTEGER NOT NULL REFERENCES fertilizer_type(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, fertilizer_id)
);
CREATE TABLE plant_season (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    season_id INTEGER NOT NULL REFERENCES season(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, season_id)
);
CREATE TABLE plant_disease (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    disease_id INTEGER NOT NULL REFERENCES disease_type(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, disease_id)
);
CREATE TABLE plant_region (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    region_id INTEGER NOT NULL REFERENCES suitable_region(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, region_id)
);
CREATE TABLE plant_method (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    method_id INTEGER NOT NULL REFERENCES cultivation_method(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, method_id)
);
CREATE TABLE plant_planttype (
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    type_id INTEGER NOT NULL REFERENCES plant_type(id) ON DELETE CASCADE,
    PRIMARY KEY (plant_id, type_id)
);
-- Bảng lưu bình luận người dùng về cây trồng
CREATE TABLE Comments (
    id SERIAL PRIMARY KEY,
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES account(id) ON DELETE CASCADE,
    content TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Bảng lưu đánh giá sao cho cây trồng
CREATE TABLE Ratings (
    id SERIAL PRIMARY KEY,
    plant_id INTEGER NOT NULL REFERENCES plant(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES account(id) ON DELETE CASCADE,
    rating_value INTEGER CHECK (rating_value BETWEEN 1 AND 5),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (plant_id, user_id)  -- mỗi người chỉ đánh giá 1 lần cho 1 cây
);
INSERT INTO Ratings (plant_id, user_id, rating_value)
VALUES
(1, 1, 5),  -- admin đánh giá Cà chua 5 sao
(1, 2, 4),  -- user01 đánh giá Cà chua 4 sao
(2, 3, 5),  -- editor01 đánh giá Rau muống 5 sao
(3, 2, 4),  -- user01 đánh giá Xoài 4 sao
(4, 1, 5);  -- admin đánh giá Cà phê 5 sao
INSERT INTO Comments (plant_id, user_id, content)
VALUES
(1, 1, 'Cà chua nhà mình trồng dễ chăm, quả mọng và vị ngọt dịu.'),
(1, 2, 'Mình trồng thử, cây phát triển nhanh và cho nhiều quả.'),
(2, 3, 'Rau muống dễ trồng, ăn giòn và ngon, hợp với bữa cơm Việt.'),
(3, 2, 'Xoài ngọt, thơm, nhưng cần nhiều nắng và đất tơi xốp.'),
(4, 1, 'Cà phê Robusta thơm mạnh, hợp gu mình.'),
(5, 3, 'Lá lốt rất thơm, thường dùng cuốn thịt nướng, dễ trồng quanh năm.');


INSERT INTO account (username, password, email, role)
VALUES 
('admin01', '1234', 'admin01@example.com', 'admin'),
('user01', '1234', 'user01@example.com', 'user'),
('editor01', '1234', 'editor01@example.com', 'user');

INSERT INTO account (username, password, email, role)
VALUES 
('user02', '1234', 'user02@example.com', 'user'),
('user03', '1234', 'user03@example.com', 'user'),
('user04', '1234', 'user04@example.com', 'user'),
('user05', '1234', 'user05@example.com', 'user'),
('admin02', '1234', 'admin02@example.com', 'admin');
INSERT INTO plant_type (type_name) VALUES
('Rau ăn lá'),
('Rau ăn quả'),
('Cây ăn trái'),
('Cây công nghiệp'),
('Cây thuốc'),
('Cây cảnh'),
('Cây lấy củ'),
('Cây lấy hạt'),
('Cây thân gỗ'),
('Cây leo giàn');

INSERT INTO season (season_name, start_month, end_month) VALUES
('Xuân', 1, 3),
('Hạ', 4, 6),
('Thu', 7, 9),
('Đông', 10, 12),
('Mùa mưa', 5, 10),
('Mùa khô', 11, 4),
('Quanh năm', 1, 12),
('Xuân-Hạ', 3, 4),
('Hạ-Thu', 6, 7),
('Thu-Đông', 9, 10);

INSERT INTO fertilizer_type (type_name, description) VALUES
('Hữu cơ', 'Phân chuồng, phân xanh, phân vi sinh'),
('Vô cơ', 'Phân hóa học tổng hợp'),
('NPK', 'Phân hỗn hợp chứa Nitơ, Phốt pho, Kali'),
('Phân vi sinh', 'Chứa vi sinh vật có lợi cho đất'),
('Phân lá', 'Phun trực tiếp lên lá cây'),
('Phân bón gốc', 'Bón vào đất trước khi trồng'),
('Phân chậm tan', 'Giải phóng dinh dưỡng từ từ'),
('Phân vi lượng', 'Cung cấp khoáng chất vi lượng'),
('Phân hữu cơ vi sinh', 'Kết hợp hữu cơ và vi sinh'),
('Phân sinh học', 'Chiết xuất từ sinh vật tự nhiên');

INSERT INTO soil_type (soil_name, description,notes) VALUES
('Đất phù sa', 'Màu mỡ, giàu dinh dưỡng, giữ ẩm tốt','Bổ sung chất hữu cơ:Nên bón thêm phân hữu cơ hoặc các chất hữu cơ khác để cải thiện kết cấu đất, tăng độ tơi xốp và độ phì nhiêu.'),
('Đất cát', 'Thoát nước nhanh, ít dinh dưỡng,Dễ canh tác,Giữ nhiệt tốt','Bón phân hữu cơ thường xuyên: Bổ sung phân trộn, phân hữu cơ hoại mục (xác thực vật, rơm rạ, phân chuồng) để tăng khả năng giữ nước và dinh dưỡng cho đất. Tưới nước đều đặn: Vì đất thoát nước nhanh, bạn cần tưới nước thường xuyên hơn để đảm bảo cây trồng có đủ nước. Cải tạo đất: Bổ sung các vật liệu hữu cơ như vỏ cây, rơm rạ đã ủ mục để cải thiện độ phì nhiêu và cấu trúc đất. Che phủ gốc cây: Dùng rơm rạ, lá khô hoặc bạt phủ để giữ ẩm, ổn định nhiệt độ đất và hạn chế sự bay hơi nước.'),
('Đất thịt', 'Giữ ẩm tốt, thoát nước trung bình, phù hợp với nhiều loại cây','Cải tạo đất trước khi trồng: Xử lý đất để loại bỏ mầm bệnh, tạp chất và tăng độ tơi xốp, bổ sung độ mùn và dinh dưỡng cho đất. Bổ sung giá thể tạo xốp: Trộn đất thịt với các vật liệu tạo xốp như vỏ trấu, xơ dừa để tăng khả năng thoát nước và làm đất tơi xốp hơn. Cung cấp phân hữu cơ: Kết hợp phân hữu cơ với đất thịt để bổ sung dinh dưỡng và cải thiện cấu trúc đất, giúp đất giữ ẩm, thoát nước tốt và cân bằng dinh dưỡng.'),
('Đất sét', 'Giữ nước và dinh dưỡng tốt, thích hợp với cây cần nhiều độ ẩm; có nhiệt độ ổn định.Khó thoát nước, dễ gây ngập úng rễ cây','Cải tạo đất:Bổ sung chất hữu cơ: Trộn phân chuồng hoai mục, phân xanh để đất tơi xốp và giàu dinh dưỡng hơn. Bổ sung cát: Trộn thêm cát để tăng độ thoát nước và tơi xốp cho đất, nhưng cần điều chỉnh lượng phù hợp. Làm luống cao: Lên luống cao hoặc làm mô trồng, kết hợp với rãnh thoát nước sẽ giúp đất thoáng khí và tránh ngập úng.'),
('Đất đỏ bazan', 'Giàu khoáng,Giàu dinh dưỡng, khoáng chất, có khả năng giữ ẩm tốt và thoát nước tốt, tạo điều kiện lý tưởng cho nhiều loại cây trồng phát triển.Thích hợp cây công nghiệp','Bổ sung dinh dưỡng: Đất đỏ bazan cần được bổ sung phân hữu cơ, phân chuồng hoai mục để tăng lượng mùn, cải thiện cấu trúc đất và cung cấp dinh dưỡng cho cây. Kiểm soát độ pH: Độ pH của đất đỏ bazan thường cao, cần kiểm tra và điều chỉnh bằng cách bón vôi nếu cần thiết để phù hợp với loại cây trồng.'),
('Đất đồi', 'Khô', 'thoát nước nhanh'),
('Đất ven biển', 'Thường mặn, cần chọn giống phù hợp','Cải tạo đất:Bổ sung chất hữu cơ: Trộn thêm phân chuồng hoai mục, trùn quế, hoặc compost để tăng độ màu mỡ, giữ ẩm cho đất.Tạo lớp phủ: Sử dụng rơm rạ, vỏ dừa, hoặc lá cây để phủ gốc giúp giữ ẩm và chống xói mòn.Tưới nước hợp lý: Giai đoạn đầu cần tưới nước đều đặn để cây đủ ẩm, sau khi cây trưởng thành có thể giảm tần suất tưới.'),
('Đất ruộng', 'Giữ ẩm và dinh dưỡng tốt: Đất ruộng giàu chất hữu cơ và có khả năng giữ nước tốt, cung cấp môi trường thuận lợi cho rễ cây phát triển.Dễ bị nén chặt và khó thoát nước: Nếu không được cải tạo, đất ruộng có thể bị chai cứng, gây cản trở sự phát triển của rễ và dẫn đến tình trạng úng nước.Độ pH không ổn định: Độ pH của đất ruộng có thể cần điều chỉnh để phù hợp với từng loại cây trồng, đảm bảo khả năng hấp thụ dinh dưỡng của cây.Thích hợp cây lúa và cây nước','cải tạo đất để tăng độ tơi xốp, thoát nước tốt, bổ sung dinh dưỡng cho cây, và đảm bảo đất không bị nhiễm mặn hoặc ngập úng. Đất ruộng có đặc điểm giữ ẩm và dinh dưỡng tốt nhưng dễ bị nén chặt, do đó cần trộn thêm vật liệu hữu cơ, phân chuồng hoai mục, xới đất thường xuyên để cải thiện cấu trúc, và điều chỉnh độ pH nếu cần thiết.');

INSERT INTO disease_type (disease_name, symptoms, causes) VALUES
('Thối rễ', 'Rễ chuyển màu đen, mềm nhũn', 'Nấm Phytophthora'),
('Vàng lá', 'Lá chuyển vàng, rụng sớm', 'Thiếu dinh dưỡng hoặc virus'),
('Nấm mốc trắng', 'Lớp mốc trắng trên thân/lá', 'Nấm Sclerotinia'),
('Đốm lá', 'Xuất hiện đốm nâu hoặc đen', 'Vi khuẩn hoặc nấm'),
('Héo rũ', 'Cây héo dù đất còn ẩm', 'Fusarium hoặc vi khuẩn'),
('Thối quả', 'Quả mềm, có mùi hôi', 'Vi khuẩn Erwinia'),
('Rệp sáp', 'Côn trùng trắng bám trên thân', 'Rệp hút nhựa cây'),
('Sâu đục thân', 'Thân cây bị khoét lỗ', 'Sâu non côn trùng'),
('Bệnh khảm', 'Lá biến dạng, loang lổ', 'Virus truyền qua côn trùng'),
('Bệnh phấn trắng', 'Lá phủ lớp bụi trắng', 'Nấm Erysiphe');

INSERT INTO diseasetreatment (disease_id, treatment_method, notes) VALUES
(1, 'Ngâm rễ bằng thuốc trừ nấm Ridomil', 'Thực hiện trước khi trồng'),
(2, 'Bổ sung phân vi lượng và phun thuốc vi sinh', 'Lặp lại sau 7 ngày'),
(3, 'Cắt bỏ phần bị bệnh và phun thuốc sinh học', 'Tránh tưới nước lên lá'),
(4, 'Phun thuốc Copper hydroxide', 'Kết hợp vệ sinh vườn'),
(5, 'Xử lý đất bằng vôi và thuốc trừ nấm', 'Luân canh cây trồng'),
(6, 'Thu gom quả hỏng và xử lý bằng thuốc sinh học', 'Không để quả tiếp xúc đất'),
(7, 'Phun dầu khoáng và thuốc trừ rệp', 'Lặp lại mỗi 10 ngày'),
(8, 'Dùng bẫy đèn và thuốc sinh học', 'Theo dõi lỗ đục trên thân'),
(9, 'Phun thuốc kháng virus và diệt côn trùng', 'Kiểm tra cây giống trước khi trồng'),
(10, 'Phun thuốc phấn trắng sinh học', 'Tránh trồng nơi ẩm thấp');

INSERT INTO suitable_region (region_name, climate_type, notes) VALUES
('Đồng bằng sông Cửu Long', 'Nhiệt đới gió mùa', 'Thích hợp cây lúa, rau màu'),
('Tây Nguyên', 'Nhiệt đới cao nguyên', 'Thích hợp cà phê, hồ tiêu'),
('Đồng bằng Bắc Bộ', 'Ôn đới cận nhiệt', 'Thích hợp rau vụ đông'),
('Duyên hải Nam Trung Bộ', 'Khô hạn', 'Thích hợp cây chịu hạn'),
('Miền núi phía Bắc', 'Lạnh, độ cao lớn', 'Thích hợp cây ăn trái ôn đới'),
('Trung du Bắc Bộ', 'Gió mùa, đất đồi', 'Thích hợp chè, cây ăn quả'),
('Đảo Phú Quốc', 'Nhiệt đới biển', 'Thích hợp cây dược liệu'),
('Vùng ven biển miền Trung', 'Gió mạnh, đất cát', 'Thích hợp cây chắn gió'),
('Vùng biên giới Tây Nam', 'Nóng ẩm', 'Thích hợp cây công nghiệp'),
('Vùng cao Tây Bắc', 'Lạnh sâu', 'Thích hợp cây ôn đới');

INSERT INTO cultivation_method (method_name, description, notes) VALUES
('Gieo hạt trực tiếp', '1.Chuẩn bị đất: Cày bừa đất thật kỹ, bón phân lót đầy đủ và làm phẳng mặt luống để tạo môi trường thuận lợi cho hạt nảy mầm. 2.Gieo hạt:Xác định vị trí: Vạch hàng theo khoảng cách tiêu chuẩn hoặc tạo hốc gieo hạt, đảm bảo khoảng cách giữa các hạt. Đặt hạt giống vào vị trí đã định. Với hạt nhỏ, có thể xoa ngón tay để rắc đều. Với hạt lớn hơn, có thể ấn nhẹ hạt xuống đất. 3.Nguyên tắc độ sâu: Nguyên tắc chung là gieo hạt sâu gấp khoảng ba lần đường kính của hạt. 4.Phủ đất và tưới nước:Phủ một lớp đất mỏng lên trên hạt để giữ ẩm và bảo vệ hạt. Tưới nước nhẹ nhàng, đảm bảo độ ẩm đủ cho hạt nảy mầm. 5.Chăm sóc:Theo dõi và giữ ẩm đều đặn cho đất. Theo hướng dẫn của gói hạt giống để đảm bảo tỷ lệ nảy mầm cao.', 'Phương pháp này phù hợp với các loại cây có rễ nhạy cảm hoặc khi muốn cây thích nghi sớm với điều kiện tự nhiên.'),
('Ươm cây con', 'Bước 1: Chuẩn bị vườn và dụng cụ ươm,khoảng trống khay ươm cho từng cây non phải đủ để đảm bảo sự phát triển của cây. Đất trồng phải có độ tơi xốp cao, đủ dinh dưỡng, không có cỏ dại.Bước 2:Sau khi lựa chọn loại hạt giống chất lượng của loài cây bạn muốn trồng, bạn nên xử lý chúng trước khi gieo. Đầu tiên, rửa sạch hạt giống. Tiếp theo ngâm hạt vào trong nước ấm (45 - 50 độ C) trong khoảng thời gian nhất định (vỏ hạt càng dày thì thời gian ngâm càng lâu. Cuối cùng chính là vớt hạt giống ra, trộn với một số loại tro bếp để khi gieo hạt không bị nát.Khi đã gieo hạt giống xuống đất, ta cần cho hạt tiếp xúc hoàn toàn với đất, đảm bảo chúng có thể tiếp nhận đầy đủ dưỡng chất từ đất để sinh trưởng. Sau đó, bạn nên phủ thêm lớp vụn trấu lên luống để tránh trường hợp dưới tác động của lực nước tưới, hạt chìm xuống quá sâu trong đất khiến chúng khó nảy mầm.Bước 3:Trong quá trình chờ hạt giống nảy mầm bạn cần cung cấp đủ dinh dưỡng và nước để chúng sinh trưởng tốt.', 'Sử dụng mái che nilon nếu ươm trong mùa mưa để tránh trôi hạt hoặc dập nát cây, giúp giữ ấm cho cây con.Thường xuyên giữ ẩm cho đất, tránh để khô hoặc quá ướt. Khi cây con lớn, tỉa bớt những cây yếu để cây phát triển đồng đều.'),
('Trồng bằng hom', '1. Chuẩn bị vườn ươm: Bố trí ở nơi cao ráo, tiện nước tưới, có mái che hoặc lưới để tránh nắng gắt và gió mạnh. Vật liệu giâm hom: Hỗn hợp giá thể như cát hoặc đất pha mùn dừa, đất phù sa. Dụng cụ: Dao, kéo sắc để cắt hom, hệ thống tưới phun sương. 2. Chọn hom: Lấy từ cây mẹ khỏe mạnh, không bị sâu bệnh. Chọn hom có kích thước phù hợp (thường từ 10-15 cm), có ít nhất 1-2 mắt lá.Cắt hom: Dùng dao sắc cắt đúng kỹ thuật để tránh làm dập nát hom.3. Xử lý: Có thể xử lý phần gốc hom bằng chất kích thích ra rễ để tăng tỷ lệ thành công.Giâm hom: Cắm hom vào bầu ươm hoặc luống giâm. Chú ý để lại các mầm và tỉa bớt lá dưới để tránh bị chôn vùi.', 'Tưới nước: Tưới phun sương tự động để giữ ẩm cho hom. Che chắn:Duy trì độ che phủ để tránh tác động trực tiếp của ánh nắng. Chăm sóc khác: Vun đất, nhổ cỏ định kỳ.'),
('Trồng thủy canh', '1.Chuẩn bị môi trường: Lót nilon đen vào đáy và thành thùng chứa để tạo môi trường thủy canh.2.Xử lý hạt giống: Ngâm hạt giống trong nước ấm vài giờ để kích thích nảy mầm.3.Gieo hạt: Trộn giá thể đã chuẩn bị vào các cốc nhựa nhỏ, sau đó rải hạt giống vào cốc.4.Pha dung dịch dinh dưỡng với nước theo đúng tỷ lệ.5.Đặt các cốc đã gieo hạt vào nắp thùng xốp đã khoét lỗ vừa với cốc.Đổ dung dịch dinh dưỡng vào thùng xốp.Đặt nắp thùng có cốc cây lên thùng xốp sao cho đáy cốc ngập trong dung dịch khoảng 1-2 cm.', 'Thay nước thường xuyên: Khoảng 7-10 ngày/lần để đảm bảo cây có đủ dinh dưỡng và oxy.Vệ sinh: Dọn dẹp sạch sẽ bình thủy canh để tránh rong rêu phát triển.Quan sát cây: Thường xuyên theo dõi để phát hiện sớm các vấn đề như rễ thối hoặc lá úa vàng.'),
('Trồng hữu cơ', '1.Chuẩn bị đất: Cải tạo đất bằng phân hữu cơ, phân ủ để đất giàu dinh dưỡng và tơi xốp. 2.Chọn giống: Chọn giống cây trồng phù hợp với điều kiện tự nhiên, ưu tiên các giống bản địa, có khả năng kháng bệnh tốt.Tưới tiêu: Đảm bảo nguồn nước tưới sạch và sử dụng hệ thống tưới tiêu hiệu quả để tiết kiệm nước. 3.Chăm sóc cây: Luôn nhổ cỏ để tránh cạnh tranh dinh dưỡng và ánh sáng với cây trồng, đồng thời tạo không gian cho cây phát triển. 4.Kiểm soát sâu bệnh: Trồng các loại cây xua đuổi côn trùng như tỏi, hành, sả để hạn chế sâu bệnh. 5.Thu hoạch: Thu hoạch đúng thời điểm để đảm bảo chất lượng sản phẩm.', 'Yêu cầu kiểm soát sâu bệnh,tạo môi trường sống cho các loài thiên địch như côn trùng có ích, chim để giúp kiểm soát sâu bệnh tự nhiên. '),
('Trồng luân canh', '1.Lập kế hoạch luân canh:Nghiên cứu về cây trồng: Tìm hiểu về nhu cầu dinh dưỡng, bộ rễ và tập quán sinh trưởng của từng loại cây bạn định trồng. Xây dựng sơ đồ: Dựa trên các thông tin trên, bạn sẽ lập ra một sơ đồ luân canh, ví dụ: trồng lúa, sau đó đến cây đậu, rồi đến cây rau màu khác. 2.Chọn cây cho vụ luân canh:Cây hút dinh dưỡng nhiều: Bắt đầu với cây trồng tiêu thụ nhiều dinh dưỡng từ đất, ví dụ như ngô.Cây bổ sung dinh dưỡng: Tiếp theo, trồng cây họ đậu (như đậu tương, đậu xanh) vì chúng có khả năng cố định nitơ, giúp bổ sung dinh dưỡng cho đất. 3.Cây cải tạo đất: Cuối cùng, chọn các loại cây khác có bộ rễ sâu hoặc cải tạo đất để cân bằng dinh dưỡng trong đất. 4.Thực hiện theo kế hoạch:Trồng cây theo trình tự: Sau khi thu hoạch cây này, bạn sẽ trồng cây tiếp theo trong sơ đồ luân canh trên chính diện tích đất đó.5. Liên tục đổi mới: Lặp lại chu trình luân canh này để đảm bảo đất luôn được cung cấp đủ dinh dưỡng và không bị cạn kiệt.', 'chọn các loại cây có nhu cầu dinh dưỡng và bộ rễ khác nhau, ưu tiên cây họ đậu để bổ sung nitơ, và xây dựng một sơ đồ luân canh khoa học cho từng vụ mùa.'),
('Trồng xen canh', '1.Chọn cây trồng phù hợp:Nguyên tắc chọn cây: Chọn cây có nhu cầu khác nhau về ánh sáng (ví dụ: cây cao, cây thấp, cây ưa sáng, cây ưa bóng) và bộ rễ (rễ nông, rễ sâu). 2.Tận dụng không gian và tài nguyên:Tối ưu diện tích: Trồng nhiều loại cây trên cùng một đơn vị diện tích để sử dụng hiệu quả không gian canh tác. 3.Tăng cường hiệu quả kinh tế và sinh học:Tăng thu nhập: Trồng xen canh giúp người nông dân có nguồn thu nhập đa dạng từ nhiều loại cây, giảm rủi ro kinh tế nếu một loại cây trồng chính bị thất bại.', 'Chọn cây trồng có mối quan hệ hỗ trợ lẫn nhau, tránh những cây cạnh tranh gay gắt về nguồn lực.'),
('Trồng trong nhà lưới', '1. Lựa chọn lưới phù hợp: Phần mái: Nên dùng lưới 16-18 mesh để chống côn trùng lớn, đảm bảo thông thoáng và chịu được mưa gió, tia UV.Phần hông (tường bao quanh): Dùng lưới dày hơn (24-50 mesh) vì côn trùng nhỏ thường xâm nhập từ dưới đất.Cửa ra vào hoặc lỗ thông gió: Cần lưới 32-50 mesh để đảm bảo kín, ngăn côn trùng hiệu quả.2. Thiết kế và xây dựng nhà lưới: Vị trí: Chọn vị trí có ánh sáng tốt, nguồn nước ổn định và tránh xa khu vực ô nhiễm.Cấu trúc: Xây dựng khung nhà lưới chắc chắn, có độ cao phù hợp để cây phát triển.Hệ thống hỗ trợ: Trang bị hệ thống tưới tiêu hiệu quả (như tưới nhỏ giọt), hệ thống thông gió, và các thiết bị khác hỗ trợ cho từng loại cây trồng.3. Chuẩn bị cho cây trồng: Chọn giống: Lựa chọn giống cây phù hợp với điều kiện khí hậu và thổ nhưỡng, ưu tiên các giống cây có thể trồng trong nhà lưới.Xử lý đất: Xử lý đất để loại bỏ mầm bệnh và cung cấp dinh dưỡng đầy đủ cho cây.Chuẩn bị cây con: Ươm cây con khỏe mạnh, không sâu bệnh để đảm bảo sự phát triển tốt nhất khi chuyển vào nhà lưới.', 'Tưới nước: Đảm bảo cung cấp đủ nước cho cây, có thể điều chỉnh lượng nước tưới tùy theo từng giai đoạn sinh trưởng của cây.Bón phân: Bổ sung dinh dưỡng hợp lý để cây phát triển tốt, có thể kết hợp bón phân qua hệ thống tưới tiêu.Kiểm soát sâu bệnh: Thường xuyên kiểm tra để phát hiện sớm sâu bệnh và xử lý kịp thời, ưu tiên các biện pháp phòng trừ sinh học.'),
('Trồng trên giá thể', '1. Chuẩn bị giá thể: Các loại giá thể phổ biến bao gồm xơ dừa, trấu hun, vỏ thông, mùn cưa, viên đất nung, đá pumice, vermiculite. Bạn có thể trộn nhiều loại giá thể để tạo hỗn hợp tối ưu về khả năng thoát nước, giữ ẩm và thoáng khí cho rễ. Đảm bảo chất lượng: Giá thể cần sạch, không chứa mầm bệnh, và có khả năng thoát nước tốt để tránh úng rễ cây. 2. Chuẩn bị hệ thống trồng: Sử dụng các vật liệu như khay, túi bầu, hoặc hệ thống giá thể xếp lớp (như trong canh tác thẳng đứng).Bố trí: Đặt giá thể vào dụng cụ trồng, đảm bảo hệ thống thoát nước hiệu quả.3. Gieo trồng Gieo hạt hoặc cây con: Trồng hạt hoặc cây con vào giá thể đã chuẩn bị.Tưới nước: Tưới đủ lượng nước cho cây, nhưng không để bị ngập úng. Tần suất tưới có thể 1-2 lần/ngày tùy thuộc vào loại cây và điều kiện thời tiết.', 'Bón phân: Cung cấp dinh dưỡng cho cây thông qua các dung dịch dinh dưỡng pha loãng hoặc các vật liệu trộn sẵn trong giá thể. Kiểm soát môi trường: Trồng trong nhà kính sẽ giúp kiểm soát tốt hơn nhiệt độ, độ ẩm, ánh sáng, và phòng trừ sâu bệnh.'),
('Trồng bán thủy canh', '1Trồng cây: Đổ giá thể đã trộn vào ngăn trên của chậu, sau đó gieo hạt hoặc đặt cây con vào. 2.Pha dung dịch: Pha dung dịch dinh dưỡng loãng với nước theo tỉ lệ được hướng dẫn, khuấy đều. 3.Tưới dung dịch: Sử dụng hệ thống tưới nhỏ giọt để tưới trực tiếp dung dịch dinh dưỡng vào gốc cây. Dung dịch sẽ thẩm thấu qua giá thể, cung cấp dưỡng chất cho rễ. 4.Kiểm tra và bổ sung: Theo dõi độ ẩm của giá thể và bổ sung dung dịch dinh dưỡng khi cần thiết, đảm bảo cây luôn có đủ nước và dưỡng chất.', ' chuẩn bị chậu có ngăn chứa dung dịch dinh dưỡng và ngăn cho cây sinh trưởng, sau đó trồng cây vào giá thể, pha dung dịch dinh dưỡng và tưới nhỏ giọt vào gốc cây. ');

INSERT INTO plant (plant_name, science_name, description, origin, growth_duration) VALUES
('Cà chua','Solanum lycopersicum', 'Cà chua là một loại rau quả thuộc họ Cà, có nguồn gốc từ Nam Mỹ, là cây thân thảo sống theo mùa. Cây có thân mọng nước, phân cành nhiều, lá xẻ lông chim và hoa màu vàng cam. Quả ban đầu có màu xanh, khi chín sẽ chuyển từ vàng đến đỏ mọng, có vị hơi chua, giàu vitamin C, A và lycopene.', 'Nam Mỹ',         90),
('Rau muống','Ipomoea aquatica','Rau muống là cây thân thảo, mọc bò trên mặt nước hoặc trên cạn, thân rỗng, lá hình tam giác hoặc mũi tên, hoa trắng hoặc hồng tím. Giàu vitamin và khoáng chất, phổ biến ở Việt Nam.', 'Đông Nam Á',     30),
('Xoài','Mangifera indica','Cây xoài là cây thân gỗ lớn, lá thuôn dài, hoa nhỏ màu vàng nhạt. Quả xoài chín có màu vàng, vị chua ngọt, thơm, giàu dinh dưỡng.', 'Ấn Độ',           150),
('Cà phê','Coffea','Cà phê là cây thân gỗ lâu năm, quả hạch đỏ, chứa hai hạt. Hạt cà phê được dùng làm thức uống phổ biến, gồm giống Arabica và Robusta.', 'Châu Phi',        240),
('Lá lốt','Piper lolot','Lá lốt là cây thân thảo sống dai, lá hình tim, mặt trên xanh bóng, có mùi thơm đặc trưng. Hoa trắng mọc ở nách lá, quả mọng chứa một hạt.', 'Việt Nam',        60),
('Cải xanh','Brassica juncea','Cải xanh là rau họ cải, thân lá xanh, vị hơi đắng, giàu vitamin C, K, A, B và chất xơ. Dễ trồng, hỗ trợ miễn dịch và tiêu hóa.', 'Trung Quốc',      45),
('Khoai lang','Ipomoea batatas','Khoai lang là cây dây leo, lá hình tim, hoa tím nhạt. Củ thuôn dài, vỏ đỏ/tím/trắng, ruột trắng/vàng/cam/tím, giàu tinh bột.', 'Nam Mỹ',          120),
('Ngô','Zea mays','Ngô là cây thảo lớn, thân thẳng, lá dài, hoa đực ở ngọn, hoa cái ở nách lá. Quả ngô có hạt cứng, bóng, dùng làm lương thực.', 'Trung Mỹ',         100),
('Đậu tương','Glycine max','Đậu tương là cây thân thảo, lá kép, hoa trắng/tím, quả hình liềm có lông mềm. Hạt giàu đạm, dùng làm thực phẩm.', 'Trung Quốc',      90),
('Mướp','Luffa cylindrica','Mướp là cây dây leo, lá hình tim hoặc tam giác, hoa vàng, quả dài hình trụ, khi non mềm xốp, khi già có xơ dai.', 'Ấn Độ',            70);

INSERT INTO plantimages (plant_id, image_url) VALUES
(1, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR1aSM6iC_vo3wUzqaREQ-NK-CiA092WuVeTA&s'),
(2, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhmncZco6wffRZh7zdy44tUDpVmWAr8qyUEQ&s'),
(3, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQpnSTMAbjkdSc3QoEk0KVs554Gwz90fpa4sQ&s'),
(4, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTVbEkGjLgDRT6Gmj9X693esVc4B0J2zTkaTw&s'),
(5, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRFa5SF9rtlBa8ExKJAmC2GbTkHJZZrXwCzyw&s'),
(6, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQxFotgmxNqGWnOEYKjlW2fFPaxS2EGHUq5XA&s'),
(7, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcREXczkGMpd6TdV9UBNwUVhFEjzloDKRHu-BA&s'),
(8, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQqyIFLAhPa98SjdI2N7dkkyOQ19xOLd-lnuw&s'),
(9, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTVp1-uaPmZB9JQaS_eQuyBIA14MTRfwiOolw&s'),
(10, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQyUm2MK4AbcbldHqufaUvUxt10ElyQqM7ftw&s');

INSERT INTO plant_planttype VALUES
(1, 2), -- Cà chua: Rau ăn quả
(2, 1), -- Rau muống: Rau ăn lá
(3, 3), -- Xoài: Cây ăn trái
(4, 4), -- Cà phê: Cây công nghiệp
(5, 5), -- Lá lốt: Cây thuốc
(6, 1), -- Cải xanh: Rau ăn lá
(7, 7), -- Khoai lang: Cây lấy củ
(8, 8), -- Ngô: Cây lấy hạt
(9, 8), -- Đậu tương: Cây lấy hạt
(10, 10); -- Mướp: Cây leo giàn

INSERT INTO plant_season VALUES
(1, 1), (1, 2), -- Cà chua: Xuân, Hạ
(2, 5), (2, 7), -- Rau muống: Mùa mưa, Quanh năm
(3, 2), (3, 3), -- Xoài: Hạ, Thu
(4, 5), (4, 6), -- Cà phê: Mùa mưa, Mùa khô
(5, 3), (5, 4), -- Lá lốt: Thu, Đông
(6, 1), (6, 7), -- Cải xanh: Xuân, Quanh năm
(7, 1), (7, 2), -- Khoai lang: Xuân, Hạ
(8, 2), (8, 3), -- Ngô: Hạ, Thu
(9, 2), (9, 3), -- Đậu tương: Hạ, Thu
(10, 1), (10, 2); -- Mướp: Xuân, Hạ

INSERT INTO plant_fertilizer VALUES
(1, 3), (1, 1), -- Cà chua: NPK, Hữu cơ
(2, 1), (2, 5), -- Rau muống: Hữu cơ, Phân lá
(3, 3), (3, 6), -- Xoài: NPK, Phân bón gốc
(4, 2), (4, 6), -- Cà phê: Vô cơ, Phân bón gốc
(5, 1), (5, 9), -- Lá lốt: Hữu cơ, Phân hữu cơ vi sinh
(6, 3), (6, 5), -- Cải xanh: NPK, Phân lá
(7, 1), (7, 7), -- Khoai lang: Hữu cơ, Phân chậm tan
(8, 3), (8, 2), -- Ngô: NPK, Vô cơ
(9, 3), (9, 4), -- Đậu tương: NPK, Phân vi sinh
(10, 1), (10, 5); -- Mướp: Hữu cơ, Phân lá

INSERT INTO plant_disease VALUES
(1, 6), (1, 4), -- Cà chua: Thối quả, Đốm lá
(2, 2), (2, 5), -- Rau muống: Vàng lá, Héo rũ
(3, 4), (3, 1), -- Xoài: Đốm lá, Thối rễ
(4, 8), (4, 7), -- Cà phê: Sâu đục thân, Rệp sáp
(5, 9), (5, 10), -- Lá lốt: Bệnh khảm, Phấn trắng
(6, 2), (6, 4), -- Cải xanh: Vàng lá, Đốm lá
(7, 1), (7, 5), -- Khoai lang: Thối rễ, Héo rũ
(8, 4), (8, 8), -- Ngô: Đốm lá, Sâu đục thân
(9, 2), (9, 9), -- Đậu tương: Vàng lá, Bệnh khảm
(10, 10), (10, 4); -- Mướp: Phấn trắng, Đốm lá

INSERT INTO plant_region VALUES
(1, 1), (1, 3), --Cà chua: ĐBSCL, ĐBBB
(2, 1), (2, 3), -- Rau muống: ĐBSCL, ĐBBB
(3, 2), (3, 5), -- Xoài: Tây Nguyên, Miền núi phía Bắc
(4, 2), (4, 9), -- Cà phê: Tây Nguyên, Biên giới Tây Nam
(5, 7), (5, 3), -- Lá lốt: Phú Quốc, ĐBBB
(6, 3), (6, 6), -- Cải xanh: ĐBBB, Trung du Bắc Bộ
(7, 1), (7, 4), -- Khoai lang: ĐBSCL, Duyên hải Nam Trung Bộ
(8, 1), (8, 5), -- Ngô: ĐBSCL, Miền núi phía Bắc
(9, 1), (9, 6), -- Đậu tương: ĐBSCL, Trung du Bắc Bộ
(10, 1), (10, 8); -- Mướp: ĐBSCL, Ven biển miền Trung

INSERT INTO plant_method VALUES
(1, 1), (1, 2), -- Cà chua: Gieo hạt, Ươm cây con
(2, 1), (2, 3), -- Rau muống: Gieo hạt, Trồng bằng hom
(3, 1), (3, 6), -- Xoài: Gieo hạt, Luân canh
(4, 1), (4, 6), -- Cà phê: Gieo hạt, Luân canh
(5, 3), (5, 5), -- Lá lốt: Trồng bằng hom, Trồng hữu cơ
(6, 1), (6, 8), -- Cải xanh: Gieo hạt, Trồng trong nhà lưới
(7, 1), (7, 6), -- Khoai lang: Gieo hạt, Luân canh
(8, 1), (8, 7), -- Ngô: Gieo hạt, Xen canh
(9, 1), (9, 4), -- Đậu tương: Gieo hạt, Thủy canh
(10, 1), (10, 10); -- Mướp: Gieo hạt, Bán thủy canh

INSERT INTO plant_soil (plant_id, soil_id) VALUES
(1, 1),
(1, 3),
(2, 1),
(2, 8),
(3, 5),
(3, 3),
(4, 5),
(4, 6),
(5, 1),
(5, 3),
(6, 1),
(6, 3),
(7, 2),
(7, 6),
(8, 2),
(8, 3),
(9, 3),
(9, 6),
(10, 2),
(10, 7);

